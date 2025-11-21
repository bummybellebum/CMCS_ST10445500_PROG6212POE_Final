//ST10445500 - PROG6212POE - Final Submission

//Models: Claim

//.......................................o0oSTART OF FILEo0o........................................//

using Microsoft.EntityFrameworkCore;

namespace CMCS_ST10445500_PROG6212POE_Final.Models
{
    //enum of claim statuses
    public enum ClaimStatus
    {
        Pending,
        Verified,
        RejectedByCoordinator,
        Approved,
        RejectedByManager
    }

    //.........................................................................//
    public class Claim
    {
        public int Id { get; set; }
        public int LecturerId { get; set; }
        public AppUser Lecturer { get; set; } = null!;

        [Precision(18, 2)]
        public decimal HoursWorked { get; set; }

        [Precision(18, 2)]
        public decimal HourlyRate { get; set; }

        [Precision(18, 2)]
        public decimal TotalAmount => HoursWorked * HourlyRate;
        public string? Notes { get; set; }
        public DateTime SubmittedDate { get; set; } = DateTime.Now;
        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;
    }
    //.........................................................................//
}

//........................................o0oEND OF FILEo0o.........................................//