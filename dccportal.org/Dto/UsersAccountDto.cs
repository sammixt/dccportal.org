using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dccportal.org.Dto
{
    public class UsersAccountDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Password is required"),MinLength(6)]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int BelieverId { get; set; }
        public Nullable<int> DeptId { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<DateTime> CreationDate { get; set; }

        public string setBelieverIdString {get; set;}

        public  BelieversDto Believer { get; set; }
        public  DepartmentDto Dept { get; set; }
        public  RolesDto Role { get; set; }
    }
}