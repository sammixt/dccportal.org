using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dccportal.org.Dto
{
    //Ties to payment entity
    public class PaymentModelDto
    {
        public int PaymentId { get; set; }
        public int? DuesId { get; set; }
        public int? MemberId { get; set; }
        [Required(ErrorMessage ="Amount is required")]
        public decimal? Amount { get; set; }
        public int? Month { get; set; }
        public string Year { get; set; }
        public DateTime? PaymentDate { get; set; }
        
        [RegularExpression("^\\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$")]
        public String PaymentDateString { get; set; }
        public DateTime? EntryDate { get; set; }
        public string ExpenseBy { get; set; }
        public string Narration { get; set; }
        public string AuthorizedBy { get; set; }
        public string TransactionType { get; set; }
        public int? DeptId { get; set; }
        public string PaymentSatus { get; set; }

        public int PaymentMethod {get; set;}

    }
}