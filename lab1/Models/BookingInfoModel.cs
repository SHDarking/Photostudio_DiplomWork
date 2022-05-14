using System;
using System.Collections.Generic;
using lab1.Dto;

namespace lab1.Models
{
    public class BookingInfoModel
    {
        public List<Hall> Halls { get; set; } = new();
        public List<Service> Services { get; set; } = new(4);
        public List<EquipmentCategory> EquipmentCategories { get; set; } = new();
        public List<Equipment> Equipments { get; set; } = new();
    }
}
