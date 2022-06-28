using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class Member
    {
        public int MemberId { get; set; }
        public int? BelieverId { get; set; }
        public int? DeptId { get; set; }
        public int? UnitId { get; set; }
        public string Status { get; set; }
        public string Groups { get; set; }
        public string ProbationStatus { get; set; }
        public string Post { get; set; }

        public virtual Believer Believer { get; set; }
        public virtual Department Dept { get; set; }
        public virtual Post PostNavigation { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
