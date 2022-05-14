using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using lab1.Models;
using lab1.Dto;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace lab1.Controllers
{
    public class ManageController : Controller
    {
        
        private PhotostudioInnoDbContext db;

        public ManageController(PhotostudioInnoDbContext context)
        {
            db = context;
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult UserAdministrationPanel()
        {
            
            List<Order> list;
            if (User.IsInRole("User"))
            {
                list = db.Orders
                    .Include(o => o.FkRenterNavigation).Include(o => o.FkStatusNavigation).Include(o => o.FkHallNavigation).ToList()
                    .Where(o => o.FkRenterNavigation.Email == User.FindFirst(x => x.Type == ClaimTypes.Email).Value).OrderBy(o => o.FkStatus)
                    .ToList();                                
            }
            else
            {
                list = db.Orders
                    .Include(o => o.FkStatusNavigation).Include(o => o.FkRenterNavigation).Include(o => o.FkHallNavigation).ToList()
                    .Where(o => o.FkStatus == 1 || o.FkStatus == 2 || o.FkStatus == 3).OrderBy(o => o.IdOrder).OrderBy(o => o.FkStatus)
                    .ToList();
                
            }
            return View(list);
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult ManageBooking(int idBooking)
        {
            ManageBookingModel model = new ManageBookingModel() { BookingInfo = new()};
            model.Order = db.Orders
                .Include(o => o.FkHallNavigation)
                .Include(o => o.FkRenterNavigation)
                .Include(o => o.OrderedEquipments)
                .Include(o => o.OrderedServices)
                .Include(o => o.FkStatusNavigation)
                .FirstOrDefault(o => o.IdOrder == idBooking);
            
            
            model.BookingInfo.Halls = db.Halls.Include(h => h.HallEquipments).ToList();
            model.BookingInfo.Services = db.Services.ToList();
            model.BookingInfo.EquipmentCategories = db.EquipmentCategories.ToList();
            model.BookingInfo.Equipments = db.Equipment.Include(e => e.FkCategoryNavigation).ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ManageBooking(ManageBookingModel model)
        {
            if (model.Form != null)
            {
                Order order = db.Orders.Include(o => o.OrderedEquipments).Include(o => o.OrderedServices).FirstOrDefault(o => o.IdOrder == model.IdBooking);

                if (order.FkHall != uint.Parse(model.Form.HallSelect))
                {
                    order.FkHall = db.Halls.FirstOrDefault(h => h.IdHall == uint.Parse(model.Form.HallSelect)).IdHall;
                }
                if (order.StartHallReserving != model.Form.GetStartHallReserving())
                {
                    order.StartHallReserving = model.Form.GetStartHallReserving();
                }
                if (order.EndHallReserving != model.Form.GetEndHallReserving())
                {
                    order.EndHallReserving = model.Form.GetEndHallReserving();
                }
                order.FkStatus = 2;

                if (order.OrderedServices.Count == 0 )
                {
                    foreach (var item in model.Form.SelectedServices)
                    {
                        order.OrderedServices
                            .Add(new OrderedService() { FkOrder = order.IdOrder, FkServices = item });
                    }   
                }
                else
                {
                    foreach (var item in order.OrderedServices)
                    {
                        if (!model.Form.SelectedServices.Contains(item.FkServices))
                        {
                            order.OrderedServices.Remove(item);
                        }
                    }
                    foreach (var item in model.Form.SelectedServices)
                    {
                        if (!order.OrderedServices.Any(os => os.FkOrder == order.IdOrder && os.FkServices == item))
                        {
                            order.OrderedServices.Add(new() { FkOrder = order.IdOrder, FkServices = item});
                        }
                    }                    
                }

                if (order.OrderedEquipments.Count == 0)
                {
                    int modifier = 0;
                    for (int i = 0; i < model.Form.SelectedEquipments.Count; i++)
                    {
                        for (int j = modifier; j < model.Form.CountEquipments.Count; j++)
                        {
                            if (model.Form.CountEquipments[j] != 0)
                            {
                                order.OrderedEquipments
                                    .Add(new OrderedEquipment() { FkOrder = order.IdOrder, FkEquipment = model.Form.SelectedEquipments[i], Amount = model.Form.CountEquipments[j] });
                                modifier = j + 1;
                                break;
                            }
                        }
                    }                    
                }
                else
                {
                    foreach (var item in order.OrderedEquipments)
                    {
                        if (!model.Form.SelectedEquipments.Contains(item.FkEquipment))
                        {
                            order.OrderedEquipments.Remove(item);
                        }                        
                    }
                    int modifier = 0;
                    foreach (var item in model.Form.SelectedEquipments)
                    {
                        
                        for (int j = modifier; j < model.Form.CountEquipments.Count; j++)
                        {
                            if (model.Form.CountEquipments[j] != 0)
                            {
                                if (!order.OrderedEquipments.Any(oe => oe.FkOrder == order.IdOrder && oe.FkEquipment == item))
                                {                                      
                                    order.OrderedEquipments.Add(new OrderedEquipment() { FkOrder = order.IdOrder, FkEquipment = item, Amount = model.Form.CountEquipments[j] });
                                }
                                else
                                { 
                                    order.OrderedEquipments.First(oe => oe.FkOrder == order.IdOrder && oe.FkEquipment == item).Amount = model.Form.CountEquipments[j];                                    
                                }                                
                                modifier = j + 1;
                                break;
                            }
                        } 
                    }
                }

                if (order.TotalCost != model.Form.TotalCost)
                {
                    order.TotalCost = model.Form.TotalCost;
                }

                db.Orders.Update(order);
                await db.SaveChangesAsync();
            }
            else
            {
                await UpdateBookingStatus(model.IdBooking);
            }

            return RedirectToAction("UserAdministrationPanel", "Manage");
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateBookingStatus(uint IdBooking)
        {
            var order = db.Orders.Include(o => o.OrderedEquipments).Include(o => o.OrderedServices).FirstOrDefault(o => o.IdOrder == IdBooking);
            if (order != null)
            {
                order.FkStatus++;
                db.Orders.Update(order);
                await db.SaveChangesAsync();
            }
            
            return RedirectToAction("UserAdministrationPanel", "Manage");
        }
        [Authorize]
        public async Task<IActionResult> DeleteBooking(int IdBooking)
        {
            var order = db.Orders.Include(o => o.OrderedEquipments).Include(o => o.OrderedServices).FirstOrDefault(o => o.IdOrder == IdBooking);
            if (order != null)
            {
                db.Orders.Remove(order);
                await db.SaveChangesAsync();
            }         
            return RedirectToAction("UserAdministrationPanel", "Manage");
        }
        [Authorize]
        public async Task<IActionResult> CancelBooking(int IdBooking)
        {
            var order = db.Orders.Include(o => o.OrderedEquipments).Include(o => o.OrderedServices).FirstOrDefault(o => o.IdOrder == IdBooking);
            if (order != null)
            {
                order.FkStatus = 5;
                db.Orders.Update(order);
                await db.SaveChangesAsync();
            }
                        
            return RedirectToAction("UserAdministrationPanel", "Manage");
        }
        
    }
}