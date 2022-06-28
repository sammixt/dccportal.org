using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class Believer
    {
        public Believer()
        {
            Attendances = new HashSet<Attendance>();
            Members = new HashSet<Member>();
            Payments = new HashSet<Payment>();
            UsersAccounts = new HashSet<UsersAccount>();
            Wallets = new HashSet<Wallet>();
        }

        public int MemberId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Sex { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? HomeAddressOne { get; set; }
        public string? HomeAddressTwo { get; set; }
        public string? City { get; set; }
        public string? StateName { get; set; }
        public string? Country { get; set; }
        public string? MaritalStatus { get; set; }
        public DateTime? WeddingAnniversary { get; set; }
        public string? FacebookName { get; set; }
        public string? InstagramHandle { get; set; }
        public string? TwitterHandle { get; set; }
        public string? AltPhoneNumber { get; set; }
        public string? Email { get; set; }
        public int? MandBid { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<UsersAccount> UsersAccounts { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
