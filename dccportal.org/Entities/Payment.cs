using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int? DuesId { get; set; }
        public int? MemberId { get; set; }
        public decimal? Amount { get; set; }
        public int? Month { get; set; }
        public string Year { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? EntryDate { get; set; }
        public string ExpenseBy { get; set; }
        public string Narration { get; set; }
        public string AuthorizedBy { get; set; }
        public string TransactionType { get; set; }
        public int? DeptId { get; set; }
        public string PaymentSatus { get; set; }

        public virtual Due Dues { get; set; }
        public virtual Believer Member { get; set; }
        public virtual Month MonthNavigation { get; set; }
    }
}
