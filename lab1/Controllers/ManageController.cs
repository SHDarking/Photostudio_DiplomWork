using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                    .Where(o => o.FkStatus == 1 || o.FkStatus == 2 || o.FkStatus == 3).OrderBy(o => o.IdOrder)
                    .ToList();
                
            }
            return View(list);
        }
        
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateBookingStatus(int idBooking)
        {
            var order = db.Orders.FirstOrDefault(o => o.IdOrder == idBooking);
            if (order != null)
            {
                order.FkStatus = order.FkStatus++;
                db.Orders.Update(order);
                await db.SaveChangesAsync();
            }
            
            return RedirectToAction("UserAdministrationPanel", "Manage");
        }
        [Authorize]
        public async Task<IActionResult> DeleteBooking(int idBooking)
        {
            var order = db.Orders.FirstOrDefault(o => o.IdOrder == idBooking);
            if (order != null)
            {
                db.Orders.Remove(order);
            }
            await db.SaveChangesAsync();            
            return RedirectToAction("UserAdministrationPanel", "Manage");
        }
        [Authorize]
        public async Task<IActionResult> CancelBooking(int idBooking)
        {
            var order = db.Orders.FirstOrDefault(o => o.IdOrder == idBooking);
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