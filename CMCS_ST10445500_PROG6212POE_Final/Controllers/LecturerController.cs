using CMCS_ST10445500_PROG6212POE_Final.Helpers;
using CMCS_ST10445500_PROG6212POE_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//ST10445500 - PROG6212POE - Final Submission

//Controllers: LecturerController

//.......................................o0oSTART OF FILEo0o........................................//

namespace CMCS_ST10445500_PROG6212POE_Final.Controllers
{
    public class LecturerController : BaseController
    {
        public LecturerController(AppDbContext db) : base(db) { }

        //.........................................................................//
        public async Task<IActionResult> Index()
        {
            var check = RequireLogin(); if (check != null) return check;
            if (RequireRole(UserRole.Lecturer) != null) return RedirectToAction("Login", "Account");

            var userId = HttpContext.Session.GetUserId().Value;
            var claims = _db.Claims.Where(c => c.LecturerId == userId).OrderByDescending(c => c.SubmittedDate).ToList();
            var user = _db.Users.Find(userId);
            ViewBag.HourlyRate = user?.HourlyRate ?? 0;
            return View(claims);
        }

        //.........................................................................//

        public async Task<IActionResult> SubmitClaim() => View();

        [HttpPost]
        public async Task<IActionResult> SubmitClaim(decimal hoursWorked, string notes)
        {
            var userId = HttpContext.Session.GetUserId().Value;
            var lecturer = _db.Users.Find(userId);

            var claim = new Claim
            {
                LecturerId = userId,
                HoursWorked = hoursWorked,
                HourlyRate = lecturer!.HourlyRate ?? 0,
                Notes = notes
            };

            _db.Claims.Add(claim);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        //.........................................................................//
    }
}

//........................................o0oEND OF FILEo0o.........................................//