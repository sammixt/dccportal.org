using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dccportal.org.Dto
{
    public class UploadUserDto
    {
        public int DeptId { get; set;}
       public IFormFile ExcelFile {get; set;}
    }

    public class UploadedUserStatus
    {
       public List<UploadedUser> Successful {get; set;} = new List<UploadedUser>();
       public List<UploadedUser> Failed {get; set;} = new List<UploadedUser>();
    }
    public class UploadedUser
    {
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Gender {get; set;}

        public string Message {get; set;}

        public string Status  {get; set;}
    }
}