using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class AdminUser
    {
        public string Id { get; set; } = null!;
        public string? Fullname { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
    }
}
