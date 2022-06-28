using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dccportal.org.Entities
{
    public class WorkerAttendance
    {
        public int Id {get; set;}

        public int Department {get; set;} 
        public DateTime Date {get; set;}
        public string DepartmentGroup {get; set;}
        public string Value { get; set; }

    }
}