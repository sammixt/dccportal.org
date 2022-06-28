using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using dccportal.org.Helper;

namespace dccportal.org.Dto
{
    public class DueDto
    {
        public int DuesId { get; set; }
        public DateTime CreationDate { get; set; }
        [Required(ErrorMessage="Dues Name is required")]
        public string DuesName { get; set; }
        public string DuesDesc { get; set; }
        [Required(ErrorMessage="Dues Type is required")]
        public string DuesType { get; set; }
        [Required(ErrorMessage="Amount is required"), Range(1,500000)]
        public decimal Amount { get; set; }
        public int DeptId { get; set; }

        public string SetDuesIdString { get; set; }

        public string GetDuesIdString {
            get {
                return Encrypter.Encrypt(Convert.ToString(DuesId),Constants.PASSPHRASE);
            }
        }
    }
}