//ST10445500 - PROG6212POE - Final Submission

//Models: AppUser

//.......................................o0oSTART OF FILEo0o........................................//

using Microsoft.EntityFrameworkCore;

namespace CMCS_ST10445500_PROG6212POE_Final.Models
{
    //enum of user roles
    public enum UserRole { Lecturer, Coordinator, Manager, HR }

    //.........................................................................//
    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        [Precision(18, 2)]
        public decimal? HourlyRate { get; set; } //only for Lecturers
    }

    //.........................................................................//
}

//........................................o0oEND OF FILEo0o.........................................//
