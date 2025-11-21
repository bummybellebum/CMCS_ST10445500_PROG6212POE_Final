using CMCS_ST10445500_PROG6212POE_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;

//ST10445500 - PROG6212POE - Final Submission

//Controllers: ClaimsApiController

//.......................................o0oSTART OF FILEo0o........................................//

namespace CMCS_ST10445500_PROG6212POE_Final.Controllers.Api
{
    [Route("api/claims")]
    [ApiController]
    public class ClaimsApiController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ClaimsApiController(AppDbContext db)
        {
            _db = db;
        }

        // Helper: Get current user from Session
        private (int? UserId, UserRole? Role) GetCurrentUser()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var roleStr = HttpContext.Session.GetString("UserRole");
            if (userId == null || roleStr == null) return (null, null);

            if (Enum.TryParse<UserRole>(roleStr, out var role))
                return (userId.Value, role);
            return (userId.Value, null);
        }

        // GET: api/claims/my-claims (Lecturer only)
        [HttpGet("my-claims")]
        public async Task<ActionResult<IEnumerable<ClaimDto>>> GetMyClaims()
        {
            var (userId, role) = GetCurrentUser();
            if (userId == null || role != UserRole.Lecturer)
                return Unauthorized("Lecturer login required.");

            var claims = await _db.Claims
                .Where(c => c.LecturerId == userId)
                .OrderByDescending(c => c.SubmittedDate)
                .Select(c => new ClaimDto
                {
                    Id = c.Id,
                    HoursWorked = c.HoursWorked,
                    HourlyRate = c.HourlyRate,
                    TotalAmount = c.HoursWorked * c.HourlyRate,
                    Status = c.Status.ToString(),
                    SubmittedDate = c.SubmittedDate,
                    Notes = c.Notes
                })
                .ToListAsync();

            return Ok(claims);
        }

        // POST: api668/claims/submit (Lecturer only)
        [HttpPost("submit")]
        public async Task<ActionResult<int>> SubmitClaim([FromBody] SubmitClaimRequest request)
        {
            var (userId, role) = GetCurrentUser();
            if (userId == null || role != UserRole.Lecturer)
                return Unauthorized("Lecturer login required.");

            if (request.HoursWorked <= 0 || request.HoursWorked > 500)
                return BadRequest("Hours must be between 0.5 and 500.");

            var lecturer = await _db.Users.FindAsync(userId);
            if (lecturer?.HourlyRate == null)
                return BadRequest("Your hourly rate has not been set by HR.");

            var claim = new Claim
            {
                LecturerId = userId.Value,
                HoursWorked = request.HoursWorked,
                HourlyRate = lecturer.HourlyRate.Value,
                Notes = request.Notes,
                SubmittedDate = DateTime.Now,
                Status = ClaimStatus.Pending
            };

            _db.Claims.Add(claim);
            await _db.SaveChangesAsync();

            return Ok(claim.Id); // Return new claim ID
        }

        // GET: api/claims/pending (Coordinator only)
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<ClaimDto>>> GetPendingClaims()
        {
            var (_, role) = GetCurrentUser();
            if (role != UserRole.Coordinator)
                return Unauthorized("Coordinator access only.");

            var claims = await _db.Claims
                .Include(c => c.Lecturer)
                .Where(c => c.Status == ClaimStatus.Pending)
                .OrderByDescending(c => c.SubmittedDate)
                .Select(c => new ClaimDto
                {
                    Id = c.Id,
                    LecturerName = c.Lecturer.Name,
                    HoursWorked = c.HoursWorked,
                    HourlyRate = c.HourlyRate,
                    TotalAmount = c.HoursWorked * c.HourlyRate,
                    SubmittedDate = c.SubmittedDate,
                    Notes = c.Notes
                })
                .ToListAsync();

            return Ok(claims);
        }

        // POST: api/claims/verify (Coordinator only)
        [HttpPost("verify/{claimId}")]
        public async Task<IActionResult> VerifyClaim(int claimId)
        {
            var (_, role) = GetCurrentUser();
            if (role != UserRole.Coordinator)
                return Unauthorized("Coordinator access only.");

            var claim = await _db.Claims.FindAsync(claimId);
            if (claim == null) return NotFound();
            if (claim.Status != ClaimStatus.Pending)
                return BadRequest("Claim is not pending.");

            claim.Status = ClaimStatus.Verified;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Claim verified successfully." });
        }

        // POST: api/claims/reject (Coordinator only)
        [HttpPost("reject/{claimId}")]
        public async Task<IActionResult> RejectClaim(int claimId, [FromBody] RejectRequest request)
        {
            var (_, role) = GetCurrentUser();
            if (role != UserRole.Coordinator)
                return Unauthorized("Coordinator access only.");

            var claim = await _db.Claims.FindAsync(claimId);
            if (claim == null) return NotFound();
            if (claim.Status != ClaimStatus.Pending)
                return BadRequest("Claim is not pending.");

            claim.Status = ClaimStatus.RejectedByCoordinator;
            claim.Notes += $"\n[Coordinator Rejection - {DateTime.Now:yyyy-MM-dd}] {request.Reason}";
            await _db.SaveChangesAsync();

            return Ok(new { message = "Claim rejected." });
        }

        // GET: api/claims/verified (Manager only)
        [HttpGet("verified")]
        public async Task<ActionResult<IEnumerable<ClaimDto>>> GetVerifiedClaims()
        {
            var (_, role) = GetCurrentUser();
            if (role != UserRole.Manager)
                return Unauthorized("Manager access only.");

            var claims = await _db.Claims
                .Include(c => c.Lecturer)
                .Where(c => c.Status == ClaimStatus.Verified)
                .Select(c => new ClaimDto
                {
                    Id = c.Id,
                    LecturerName = c.Lecturer.Name,
                    HoursWorked = c.HoursWorked,
                    HourlyRate = c.HourlyRate,
                    TotalAmount = c.HoursWorked * c.HourlyRate,
                    SubmittedDate = c.SubmittedDate,
                    Notes = c.Notes
                })
                .ToListAsync();

            return Ok(claims);
        }

        // POST: api/claims/approve/{claimId} (Manager only)
        [HttpPost("approve/{claimId}")]
        public async Task<IActionResult> ApproveClaim(int claimId)
        {
            var (_, role) = GetCurrentUser();
            if (role != UserRole.Manager)
                return Unauthorized("Manager access only.");

            var claim = await _db.Claims.FindAsync(claimId);
            if (claim == null || claim.Status != ClaimStatus.Verified)
                return BadRequest("Invalid claim.");

            claim.Status = ClaimStatus.Approved;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Claim approved for payment." });
        }

        // POST: api/claims/reject-manager/{claimId} (Manager only)
        [HttpPost("reject-manager/{claimId}")]
        public async Task<IActionResult> RejectByManager(int claimId, [FromBody] RejectRequest request)
        {
            var (_, role) = GetCurrentUser();
            if (role != UserRole.Manager)
                return Unauthorized("Manager access only.");

            var claim = await _db.Claims.FindAsync(claimId);
            if (claim == null || claim.Status != ClaimStatus.Verified)
                return BadRequest("Invalid claim.");

            claim.Status = ClaimStatus.RejectedByManager;
            claim.Notes += $"\n[Manager Rejection - {DateTime.Now:yyyy-MM-dd}] {request.Reason}";
            await _db.SaveChangesAsync();

            return Ok(new { message = "Claim rejected by manager." });
        }
    }

    // DTOs (Data Transfer Objects) — clean API responses
    public class ClaimDto
    {
        public int Id { get; set; }
        public string? LecturerName { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime SubmittedDate { get; set; }
        public string? Notes { get; set; }
    }

    public class SubmitClaimRequest
    {
        public decimal HoursWorked { get; set; }
        public string? Notes { get; set; }
    }

    public class RejectRequest
    {
        public string Reason { get; set; } = "No reason provided.";
    }
}

//........................................o0oEND OF FILEo0o.........................................//