using System;
using System.Collections.Generic;
using lab1.Dto;

namespace lab1.Models
{
    public class CreateBookingModel
    {
        public string HallSelect { get; set; }
        public DateTime StartDateReserving {get; set; }
        public int StartTimeReserving { get; set; }
        public DateTime EndDateReserving { get; set; }
        public int EndTimeReserving { get; set; }
        public DateTime CreatingDate { get; set; } = DateTime.Now;
        // тестовый вариант группы чекбоксов сервисов и оборудования, привязка объекта с полями и его булеан параметра на боксе
        public List<Hall> Halls { get; set; } = new();
        public List<Service> Services { get; set; } = new(4);
        
        public List<uint> SelectedServices { get; set; } = new(4);
        public List<EquipmentCategory> EquipmentCategories { get; set; } = new();
        public List<Equipment> Equipments { get; set; } = new();
        public List<uint> SelectedEquipments { get; set; } = new();

        //---------------------------------------------------------------------------------------------------------------------
        public uint TotalCost { get; set; }

        public DateTime GetStartHallReserving()
        {
            return StartDateReserving.AddHours(StartTimeReserving);
        }

        public DateTime GetEndHallReserving()
        {
            return EndDateReserving.AddHours(EndTimeReserving);
        }
    }
}