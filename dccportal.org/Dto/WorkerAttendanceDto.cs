using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using dccportal.org.Helper;

namespace dccportal.org.Dto
{
    public class WorkerAttendanceDto
    {
        public int Id {get; set;}

        public int Department {get; set;} 
        public DateTime Date {get; set;}
        
         [Required(ErrorMessage ="Group is Required")]
        public string DepartmentGroup {get; set;}
        public string Value { get; set; }

        [RegularExpression("^\\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$")]
        [Required(ErrorMessage ="Attendance Date is Required")]
        public string SetAttendanceDate {get; set;}
    }

    public class WorkersAttendanceRegister
    {
        public string FirstName {get; set;}
        public string LastName { get; set;}
        public string Group {get; set;}
        public bool Status {get; set;}
        public int MemberId {get; set;}
        public int BelieverId {get; set;}

    }

    public class WorkerAttendanceOutput
    {
        public int Id {get; set;}
        public int Department {get; set;} 
        public DateTime Date {get; set;}

        public string GetDate {
            get{
                return Date.ToLongDateString();
            }
        }
        public string DepartmentGroup {get; set;}

        public string GetIdString {
            get{
                return Encrypter.Encrypt(Convert.ToString(Id),Constants.PASSPHRASE);
            }
        }
      public  List<WorkersAttendanceRegister> AttendanceRegisters {get; set;} = new List<WorkersAttendanceRegister>();
    }

    public class AttendanceStatusUpdate
    {
        public bool Status {get; set;}
        public int MemberId {get; set;}

        public string GetAttandanceIdString {get; set;}

        public int AttendanceId {
            get
            {
                var idString = Encrypter.Decrypt(GetAttandanceIdString,Constants.PASSPHRASE);
                int id = Convert.ToInt32(idString);
                return id;
            }
        }
    }
}