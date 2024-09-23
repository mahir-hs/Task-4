using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegLog.Model
{
    public class ApplicationUser:IdentityUser
    {

        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime CurrentLoginDate { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
    }
    public enum UserStatus
    {
        Active,
        Blocked
    }
}
