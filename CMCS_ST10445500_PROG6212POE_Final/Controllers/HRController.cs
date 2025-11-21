using CMCS_ST10445500_PROG6212POE_Final.Models;
using CMCS_ST10445500_PROG6212POE_Final.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;

//ST10445500 - PROG6212POE - Final Submission

//Controllers: HRController

//.......................................o0oSTART OF FILEo0o........................................//

namespace CMCS_ST10445500_PROG6212POE_Final.Controllers
{
    public class HRController : BaseController
    {
        public HRController(AppDbContext db) : base(db) { }

        public async Task<IActionResult> Index()
        {
            var check = RequireLogin(); if (check != null) return check;
            if (RequireRole(UserRole.HR) != null) return RedirectToAction("Login", "Account");
            return View();
        }

        //.........................................................................//

        public IActionResult CreateUser() => View();

        [HttpPost]
        public async Task<IActionResult> CreateUser(string name, string email, string password, UserRole role, decimal? hourlyRate)
        {
            var user = new AppUser
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role,
                HourlyRate = role == UserRole.Lecturer ? hourlyRate : null
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //.........................................................................//

        public async Task<IActionResult> ApprovedClaimsReport()
        {
            var claims = await _db.Claims
                .Include(c => c.Lecturer)
                .Where(c => c.Status == ClaimStatus.Approved)
                .ToListAsync();

            return View(claims);
        }

        //.........................................................................//

        [HttpGet]
        public IActionResult DownloadReportPdf()
        {
            var claims = _db.Claims
                .Include(c => c.Lecturer)
                .Where(c => c.Status == ClaimStatus.Approved)
                .ToList();

            // NEW CORRECT WAY (QuestPDF 2024+ / 2025)
            var document = ReportGenerator.GenerateApprovedClaimsReport(claims);
            var pdfBytes = document.GeneratePdf();   // This method DOES exist

            return File(pdfBytes, "application/pdf", $"Approved_Claims_Report_{DateTime.Now:yyyyMMdd}.pdf");
        }

        //.........................................................................//
    }
}

//........................................o0oEND OF FILEo0o.........................................//
