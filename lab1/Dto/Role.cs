using System;
using System.Collections.Generic;

#nullable disable

namespace lab1.Dto
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public uint IdRole { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
