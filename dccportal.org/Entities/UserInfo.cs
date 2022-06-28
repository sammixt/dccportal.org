using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class UserInfo
    {
        public string Id { get; set; } = null!;
        public string? Country { get; set; }
        public string? CountryCode { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? DayOfBirth { get; set; }
        public string? MonthOfBirth { get; set; }
        public string? YearOfBirth { get; set; }
        public string? EmailAddress { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? ContactNumber { get; set; }
        public string? AddressOne { get; set; }
        public string? AddressTwo { get; set; }
        public string? AddressThree { get; set; }
        public string? Town { get; set; }
        public string? State { get; set; }
        public string? PostCode { get; set; }
        public bool IsLockedOut { get; set; }
        public string? Title { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
