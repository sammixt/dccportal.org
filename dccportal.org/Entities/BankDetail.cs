using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class BankDetail
    {
        public string Id { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? UserId { get; set; }
    }
}
