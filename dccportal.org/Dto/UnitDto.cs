using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using dccportal.org.Helper;

namespace dccportal.org.Dto
{
    public class UnitDto
    {
        public int UnitId { get; set; }
        public int? DeptId { get; set; }
        [Required(ErrorMessage = "Unit Name is required")]
        public string UnitName { get; set; }
        public string UnitFunction { get; set; }
        public string UnitShortCode { get; set; }

        public  string Department { get; set; }

        public string SetUnitIdString {get; set;}
        public string GetUnitIdString {
            get
            {
                return Encrypter.Encrypt(Convert.ToString(UnitId),Constants.PASSPHRASE);
            }
        }
    }

    public class UnitDashboardDto
    {
        public string UnitName {get; set;}

        public int Members {get; set;}

        public string UnitFunction {get; set;}
    }
}