using System;
using System.Collections.Generic;

#nullable disable

namespace lab1.Dto
{
    public partial class OrderedService
    {
        public uint FkOrder { get; set; }
        public uint FkServices { get; set; }

        public virtual Order FkOrderNavigation { get; set; }
        public virtual Service FkServicesNavigation { get; set; }
    }
}
