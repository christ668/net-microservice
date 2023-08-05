using common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Data.UserData
{
    public class UserData
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string ActiveAddress { get; set; }
        public string NIK { get; set; }
        public string MaritalStatus { get; set; }

        public UserData()
        {

        }
        public UserData(User model)
        {
            Id = model.Id;
            FirstName = model.FirstName;
            LastName = model.LastName;
            Username = model.Username;
            Password = model.Password;
            EnrollmentDate = model.EnrollmentDate;
            ActiveAddress = model.Userdetail!.ActiveAddress!;
            NIK = model.Userdetail!.Nik!;
            MaritalStatus = model.Userdetail!.MaritalStatus!;
        }
    }
}
