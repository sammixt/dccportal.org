using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dccportal.org.Helper;

namespace dccportal.org.Dto
{
    public class MemberDepartmentDto
    {
        public int MemberId {get; set;}
        public string DepartmentName {get; set;}
        public string UnitName {get; set;}
        public string Group {get; set;}
        public string Status {get; set;}
        public string Post {get; set;}

        public int DeptId {get; set;}

        public string SetMemberIdString{get; set;}
        public string GetMemberIdString {
            get {
                return Encrypter.Encrypt(Convert.ToString(MemberId),Constants.PASSPHRASE);
            }
        }

        public string SetDeptIdString{get; set;}
        public string GetDeptIdString {
            get {
                return Encrypter.Encrypt(Convert.ToString(DeptId),Constants.PASSPHRASE);
            }
        }

    }
}