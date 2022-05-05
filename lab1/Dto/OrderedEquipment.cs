using System;
using System.Collections.Generic;

#nullable disable

namespace lab1.Dto
{
    public partial class OrderedEquipment
    {
        public uint FkOrder { get; set; }
        public uint FkEquipment { get; set; }
        public uint Amount { get; set; }

        public virtual Equipment FkEquipmentNavigation { get; set; }
        public virtual Order FkOrderNavigation { get; set; }
    }
}
