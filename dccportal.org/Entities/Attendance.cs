using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class Attendance
    {
        public int AttendanceId { get; set; }
        public DateTime? DateOfDate { get; set; }
        public int? WorkerId { get; set; }
        public string? DeptGroup { get; set; }
        public bool? PresentAbsent { get; set; }
        public int Dept { get; set; }

        public virtual Believer? Worker { get; set; }
    }
}
