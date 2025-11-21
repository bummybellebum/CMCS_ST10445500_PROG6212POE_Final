using CMCS_ST10445500_PROG6212POE_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//ST10445500 - PROG6212POE - Final Submission

//Controllers: CoordinatorController

//.......................................o0oSTART OF FILEo0o........................................//

namespace CMCS_ST10445500_PROG6212POE_Final.Controllers
{
    public class CoordinatorController : BaseController
    {
        public CoordinatorController(AppDbContext db) : base(db) { }

        //................................................................//

        public async Task<IActionResult> Index()
        {
            var check = RequireLogin(); if (check != null) return check;
            if (RequireRole(UserRole.Coordinator) != null) return RedirectToAction("Login", "Account");

            var claims = _db.Claims
                .Include(c => c.Lecturer)
                .Where(c => c.Status == ClaimStatus.Pending)
                .ToList();
            return View(claims);
        }

        //................................................................//

        [HttpPost]
        public async Task<IActionResult> Verify(int claimId)
        {
            var claim = _db.Claims.Find(claimId);
            if (claim != null) claim.Status = ClaimStatus.Verified;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //................................................................//

        [HttpPost]
        public async Task<IActionResult> Reject(int claimId)
        {
            var claim = _db.Claims.Find(claimId);
            if (claim != null) claim.Status = ClaimStatus.RejectedByCoordinator;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //................................................................//
    }
}

//........................................o0oEND OF FILEo0o.........................................//