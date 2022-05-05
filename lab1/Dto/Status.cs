using System;
using System.Collections.Generic;

#nullable disable

namespace lab1.Dto
{
    public partial class Status
    {
        public Status()
        {
            Orders = new HashSet<Order>();
        }

        public uint IdStatus { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
