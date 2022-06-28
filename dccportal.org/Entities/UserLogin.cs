using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class UserLogin
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public bool ConcurrencyStamp { get; set; }
        public int AccessFailedCount { get; set; }
        public string EmailConfirmLink { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string? UserId { get; set; }
    }
}
