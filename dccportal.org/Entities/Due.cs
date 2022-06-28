using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class Due
    {
        public Due()
        {
            Payments = new HashSet<Payment>();
        }

        public int DuesId { get; set; }
        public DateTime CreationDate { get; set; }
        public string DuesName { get; set; }
        public string DuesDesc { get; set; }
        public string DuesType { get; set; }
        public decimal Amount { get; set; }
        public int DeptId { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
