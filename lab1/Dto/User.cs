using System;
using System.Collections.Generic;

#nullable disable

namespace lab1.Dto
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public uint IdUser { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public uint FkRole { get; set; }

        public virtual Role FkRoleNavigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
