using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dccportal.org.Dto
{
    public class ExcelList
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
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
    }
}