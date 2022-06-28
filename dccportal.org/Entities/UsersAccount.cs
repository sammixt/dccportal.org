using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class UsersAccount
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; }
        public int? BelieverId { get; set; }
        public int? DeptId { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual Believer Believer { get; set; }
        public virtual Department Dept { get; set; }
        public virtual Role Role { get; set; }
    }
}
