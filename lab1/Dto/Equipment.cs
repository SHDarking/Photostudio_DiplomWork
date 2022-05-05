using System;
using System.Collections.Generic;

#nullable disable

namespace lab1.Dto
{
    public partial class Equipment
    {
        public Equipment()
        {
            HallEquipments = new HashSet<HallEquipment>();
            OrderedEquipments = new HashSet<OrderedEquipment>();
        }

        public uint IdEquipment { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public uint Count { get; set; }
        public uint FkCategory { get; set; }

        public virtual EquipmentCategory FkCategoryNavigation { get; set; }
        public virtual ICollection<HallEquipment> HallEquipments { get; set; }
        public virtual ICollection<OrderedEquipment> OrderedEquipments { get; set; }
    }
}
