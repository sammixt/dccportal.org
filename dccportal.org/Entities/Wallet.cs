using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class Wallet
    {
        public int WalletId { get; set; }
        public int? MemberId { get; set; }
        public decimal? Amount { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int DeptId { get; set; }

        public virtual Believer Member { get; set; }
    }
}
