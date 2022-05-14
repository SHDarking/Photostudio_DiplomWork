using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using lab1.Dto;
using lab1.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace lab1.Controllers
{
    public class Reserving : Controller
    {
        private PhotostudioInnoDbContext db;

        public Reserving(PhotostudioInnoDbContext context)
        {
            db = context;
        }

        [HttpGet]// GET}
        public IActionResult CreateBooking()
        {
            List<Hall> list = db.Halls.Include(h => h.HallEquipments).ToList();
                        
            var model = new CreateBookingModel();
            model.Halls = list;
            model.Services = db.Services.ToList();
            model.EquipmentCategories = db.EquipmentCategories.ToList();
            model.Equipments = db.Equipment.Include(e => e.FkCategoryNavigation).ToList();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingModel model)
        {
            try
            {
                // creating new order from view model
                Order order = new Order();
                string userEmail = User.FindFirst(x => x.Type == ClaimTypes.Email).Value;

                order.FkRenter = db.Users.FirstOrDefault(u => u.Email == userEmail).IdUser;
                order.FkHall = db.Halls.FirstOrDefault(h => h.IdHall == uint.Parse(model.HallSelect)).IdHall;
                order.StartHallReserving = model.GetStartHallReserving();
                order.EndHallReserving = model.GetEndHallReserving();
                

                if (
                    order.StartHallReserving >= order.EndHallReserving &&
                    order.StartHallReserving <= DateTime.Now &&
                    !IsFreeHall(order.StartHallReserving, order.EndHallReserving)
                    )
                {
                    throw new ArgumentException("start Date & Time hall reserving must not equal end Date & Time hall reserving ");
                }
                order.CreatingDate = model.CreatingDate;
                order.FkStatus = 1;
                
                order.OrderedServices = new List<OrderedService>();
                for (int i = 0; i < model.SelectedServices.Count; i++)
                {
                    order.OrderedServices
                        .Add(new OrderedService() { FkOrder = order.IdOrder, FkServices = model.SelectedServices[i] });
                }
                
                order.OrderedEquipments = new List<OrderedEquipment>();
                int modifier = 0;
                for (int i = 0; i < model.SelectedEquipments.Count; i++)
                {
                    for (int j = modifier; j < model.CountEquipments.Count; j++)
                    {
                        if (model.CountEquipments[j] != 0)
                        {
                            order.OrderedEquipments
                                .Add(new OrderedEquipment() { FkOrder = order.IdOrder, FkEquipment = model.SelectedEquipments[i], Amount = model.CountEquipments[j] });
                            modifier = j + 1;
                            break;
                        }
                    }
                    
                }

                // calculation total cost from order data

                order.TotalCost = 0;
                uint rentDuration = (uint)(order.EndHallReserving - order.StartHallReserving).TotalHours;
                // calculation ordered services cost in order
                foreach (var item in order.OrderedServices)
                {
                    order.TotalCost += db.Services.Find(item.FkServices).Price;
                }

                // calculation ordered hall cost in order
                order.TotalCost += db.Halls.Find(order.FkHall).Price * rentDuration;

                // calculation ordered equipments cost in order
                foreach (var item in order.OrderedEquipments)
                {
                    order.TotalCost += (uint)db.Equipment.Find(item.FkEquipment).Price * rentDuration * item.Amount;
                }
                if (order.TotalCost != model.TotalCost)
                    throw new ArgumentException("Input data of total cost dont equal with calc cost");
                // writing new order into database
                db.Orders.Add(order);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("CreateBooking","Reserving"); // temporary solution
            }

            return RedirectToAction("UserAdministrationPanel", "Manage");           
            
        }

        public IActionResult GetFreedomHallTime() // обработка ajax запросов
        {            
            int id = int.Parse(Request.Query["id"]);
            int action = int.Parse(Request.Query["action"]); // получаем 3 критерия поиска дат по залам
            DateTime date = DateTime.MinValue;
            if (action != 2)
            {
                date = DateTime.Parse(Request.Query["date"]);
            }


            DateTime dayStartFindWeek;
            DateTime dayEndFindWeek;  // переменные хранения дат задающих промежуток

            GetStartAndEndWeekDays(date, action, out dayStartFindWeek, out dayEndFindWeek); // определяем промежуток в недели для вывода на экран

            var listDates = db.Orders
                .Where(o => o.FkHall == id)
                .Where(o => (new uint[] { 1,2,3}).Contains(o.FkStatus))
                .Where(o => o.StartHallReserving >= dayStartFindWeek && o.StartHallReserving <= dayEndFindWeek)
                .Select(o => new DateTime[] { o.StartHallReserving,o.EndHallReserving}).ToList();
            Calendar calendar = new Calendar(id, dayStartFindWeek, dayEndFindWeek, listDates);
            return PartialView(calendar); // генерация нового календарика для клиента
        }
        private void GetStartAndEndWeekDays(DateTime date, int action, out DateTime startWeek, out DateTime endWeek)
        {
            if (action == 1)
            {
                startWeek = date.AddDays(-7);
                endWeek = date.AddDays(-1).AddHours(23);
            }
            else if (action == 2)
            {
                DateTime nowDate = DateTime.Now.Date;
                int dayOfWeek = (int)nowDate.DayOfWeek;

                if (dayOfWeek == 0)
                {
                    dayOfWeek = 7;
                }

                startWeek = nowDate.AddDays(-(dayOfWeek - 1));
                endWeek = startWeek.AddDays(6).AddHours(23);
            }
            else if (action == 3)
            {
                startWeek = date.AddDays(7);
                endWeek = startWeek.AddDays(6).AddHours(23);
            }
            else
            {
                startWeek = DateTime.MinValue;
                endWeek = DateTime.MinValue;
            }
        }

        private bool IsFreeHall(DateTime startRent, DateTime endRent)
        {
            int count = db.Orders.Where(o => startRent < o.EndHallReserving && endRent > o.StartHallReserving).Count();
            if (count > 0)
            {
                return false;
            }
            return true;
        }
    }
}