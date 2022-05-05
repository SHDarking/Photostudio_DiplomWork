using System;
using System.Collections.Generic;

#nullable disable

namespace lab1.Dto
{
    public partial class Service
    {
        public Service()
        {
            OrderedServices = new HashSet<OrderedService>();
        }

        public uint IdService { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public uint Price { get; set; }

        public virtual ICollection<OrderedService> OrderedServices { get; set; }
    }
}
