using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dccportal.org.Dto
{
    public class MemberDto
    {
        public int MemberId { get; set; }
        public int? BelieverId { get; set; }
        public int? DeptId { get; set; }
        public int? UnitId { get; set; }
        public string Status { get; set; }
        public string Groups { get; set; }
        public string ProbationStatus { get; set; }
        public string Post { get; set; }

        public string BelieverIdString {get; set;}
    }
}