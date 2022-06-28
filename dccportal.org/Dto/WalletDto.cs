using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dccportal.org.Dto
{
    public class WalletDto
    {
         public int WalletId { get; set; }
        public int? MemberId { get; set; }
        public decimal? Amount { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int DeptId { get; set; }
        public virtual BelieversDto Member { get; set; }
    }
}