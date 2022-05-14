using System;
using System.Collections.Generic;

namespace lab1.Models
{
    public class FormModel
    {
        public string HallSelect { get; set; }
        public DateTime StartDateReserving { get; set; }
        public int StartTimeReserving { get; set; }
        public DateTime EndDateReserving { get; set; }
        public int EndTimeReserving { get; set; }
        public DateTime CreatingDate { get; set; } = DateTime.Now;

        public List<uint> SelectedServices { get; set; } = new(4);
       
        public List<uint> SelectedEquipments { get; set; } = new();

        public List<uint> CountEquipments { get; set; } = new();


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
