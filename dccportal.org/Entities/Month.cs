using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class Month
    {
        public Month()
        {
            Payments = new HashSet<Payment>();
        }

        public int MonthId { get; set; }
        public string Month1 { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
