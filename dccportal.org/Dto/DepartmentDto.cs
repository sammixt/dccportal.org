using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using dccportal.org.Helper;

namespace dccportal.org.Dto
{
    public class DepartmentDto
    {
        public int DeptId { get; set; }
        [Required(ErrorMessage ="Department name is required")]
        public string DeptName { get; set; }
        public string DeptDesc { get; set; }
        public string Vision { get; set; }
        public string ShortCode { get; set; }

         public string SetDeptIdString { get; set; }

        public string DeptIdString {
            get {
                return Encrypter.Encrypt(Convert.ToString(DeptId),Constants.PASSPHRASE);
            }
        }
    }
}