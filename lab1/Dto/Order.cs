using System;
using System.Collections.Generic;

#nullable disable

namespace lab1.Dto
{
    public partial class Order
    {
        public Order()
        {
            OrderedEquipments = new HashSet<OrderedEquipment>();
            OrderedServices = new HashSet<OrderedService>();
        }

        public uint IdOrder { get; set; }
        public uint FkRenter { get; set; }
        public DateTime CreatingDate { get; set; }
        public uint FkHall { get; set; }
        public DateTime StartHallReserving { get; set; }
        public DateTime EndHallReserving { get; set; }
        public uint TotalCost { get; set; }
        public uint FkStatus { get; set; }

        public virtual Hall FkHallNavigation { get; set; }
        public virtual User FkRenterNavigation { get; set; }
        public virtual Status FkStatusNavigation { get; set; }
        public virtual ICollection<OrderedEquipment> OrderedEquipments { get; set; }
        public virtual ICollection<OrderedService> OrderedServices { get; set; }
    }
}
