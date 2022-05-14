using System;
using System.Collections.Generic;
using lab1.Dto;

namespace lab1.Models
{
    public class ManageBookingModel
    {
        public Order Order { get; set; }
        public uint IdBooking { get; set; }
        public BookingInfoModel BookingInfo { get; set; }
        public FormModel Form { get; set; }

        
    }
}
