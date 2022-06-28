using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class Role
    {
        public Role()
        {
            UsersAccounts = new HashSet<UsersAccount>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int? RoleType { get; set; }

        public virtual ICollection<UsersAccount> UsersAccounts { get; set; }
    }
}
