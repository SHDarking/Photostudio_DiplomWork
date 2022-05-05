using System;
using System.Collections.Generic;

#nullable disable

namespace lab1.Dto
{
    public partial class Hall
    {
        public Hall()
        {
            HallEquipments = new HashSet<HallEquipment>();
            Orders = new HashSet<Order>();
        }

        public uint IdHall { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public uint Price { get; set; }
        public string Image { get; set; }

        public virtual ICollection<HallEquipment> HallEquipments { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
