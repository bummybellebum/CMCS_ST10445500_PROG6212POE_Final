using CMCS_ST10445500_PROG6212POE_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//ST10445500 - PROG6212POE - Final Submission

//Controllers: ManagerController

//.......................................o0oSTART OF FILEo0o........................................//

namespace CMCS_ST10445500_PROG6212POE_Final.Controllers
{
    public class ManagerController : Controller
    {
        private readonly AppDbContext _db;

        public ManagerController(AppDbContext db)
        {
            _db = db;
        }

        // Helper: Redirect to login if not authenticated or wrong role
        private IActionResult CheckAccess()
        {
            var roleStr = HttpContext.Session.GetString("UserRole");
            if (roleStr != UserRole.Manager.ToString())
                return RedirectToAction("Login", "Account");
            return null!;
        }

        public async Task<IActionResult> Index()
        {
            var access = CheckAccess();
            if (access != null) return access;

            // Only show VERIFIED claims (Coordinator has done their job)
            var claims = await _db.Claims
                .Include(c => c.Lecturer)
                .Where(c => c.Status == ClaimStatus.Verified)
                .OrderByDescending(c => c.SubmittedDate)
                .ToListAsync();

            ViewBag.PageTitle = "Academic Manager - Approve/Reject Claims";
            return View(claims);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int claimId)
        {
            var access = CheckAccess();
            if (access != null) return access;

            var claim = await _db.Claims.FindAsync(claimId);
            if (claim != null && claim.Status == ClaimStatus.Verified)
            {
                claim.Status = ClaimStatus.Approved;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int claimId, string rejectReason = "No reason provided")
        {
            var access = CheckAccess();
            if (access != null) return access;

            var claim = await _db.Claims.FindAsync(claimId);
            if (claim != null && claim.Status == ClaimStatus.Verified)
            {
                claim.Status = ClaimStatus.RejectedByManager;
                claim.Notes += $"\n[Manager Rejection - {DateTime.Now:yyyy-MM-dd HH:mm}] {rejectReason}";
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}

//........................................o0oEND OF FILEo0o.........................................//
