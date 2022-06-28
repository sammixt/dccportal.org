using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class Department
    {
        public Department()
        {
            Members = new HashSet<Member>();
            Units = new HashSet<Unit>();
            UsersAccounts = new HashSet<UsersAccount>();
        }

        public int DeptId { get; set; }
        public string? DeptName { get; set; }
        public string? DeptDesc { get; set; }
        public string? Vision { get; set; }
        public string? ShortCode { get; set; }

        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
        public virtual ICollection<UsersAccount> UsersAccounts { get; set; }
    
    }
}
