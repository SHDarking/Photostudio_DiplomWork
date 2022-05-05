using System;
using System.Collections.Generic;

#nullable disable

namespace lab1.Dto
{
    public partial class EquipmentCategory
    {
        public EquipmentCategory()
        {
            Equipment = new HashSet<Equipment>();
        }

        public uint IdEquipmentCategory { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Equipment> Equipment { get; set; }
    }
}
