using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using dccportal.org.Helper;

namespace dccportal.org.Dto
{
    public class BelieversDto
    {
        public int MemberId { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Sex { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression("\\d{7,15}")]
        public string PhoneNumber { get; set; }
        public string HomeAddressOne { get; set; }
        public string HomeAddressTwo { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string Country { get; set; }
        public string MaritalStatus { get; set; }
        public Nullable<DateTime> WeddingAnniversary { get; set; }
        public string FacebookName { get; set; }
        public string InstagramHandle { get; set; }
        public string TwitterHandle { get; set; }
        public string AltPhoneNumber { get; set; }
        public string Email { get; set; }
        public Nullable<int> MandBId { get; set; }

        public string SetMemberIdString { get; set; }

        public string MemberIdString {
            get {
                return Encrypter.Encrypt(Convert.ToString(MemberId),Constants.PASSPHRASE);
            }
        }

        [RegularExpression("^\\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$")]
        public string SetDateOfBirth {get; set;}
        public string DateOfBirthString { 
            get{
                return DateOfBirth.HasValue ? DateOfBirth.Value.ToString("yyyy-MM-dd") : "";
            }
        }
        [RegularExpression("^\\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$")]
        public string SetWeddingAnniversary {get; set;}
        public string WeddingAnniversaryString { 
            get{
                return WeddingAnniversary.HasValue ? WeddingAnniversary.Value.ToString("yyyy-MM-dd") : "";
            }
        }

        public int MemberInDeptId {get; set;}

        public string SetMemberInDeptIdString { get; set; }

        public string GetMemberInDeptIdString {
            get {
                return Encrypter.Encrypt(Convert.ToString(MemberInDeptId),Constants.PASSPHRASE);
            }
        }

        public int DeptId {get; set;}
    }
}