using System;
using System.Collections.Generic;

#nullable disable

namespace lab1.Dto
{
    public partial class HallEquipment
    {
        public uint FkHall { get; set; }
        public uint FkEquipment { get; set; }
        public uint Count { get; set; }

        public virtual Equipment FkEquipmentNavigation { get; set; }
        public virtual Hall FkHallNavigation { get; set; }
    }
}
