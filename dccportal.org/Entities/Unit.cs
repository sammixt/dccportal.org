using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class Unit
    {
        public Unit()
        {
            Members = new HashSet<Member>();
        }

        public int UnitId { get; set; }
        public int? DeptId { get; set; }
        public string UnitName { get; set; }
        public string UnitFunction { get; set; }
        public string UnitShortCode { get; set; }

        public virtual Department Dept { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}
