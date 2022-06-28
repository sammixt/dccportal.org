using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dccportal.org.Dto
{
    public class PaymentDto
    {
         public string DueType { get; set; }
        public decimal Amount { get; set; }
        public string Month { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string TransactionType { get; set; }
        public string Year { get; set; }
        public string PaymentStatus { get; set; }
    }
}