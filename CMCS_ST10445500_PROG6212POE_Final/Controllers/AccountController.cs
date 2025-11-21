using CMCS_ST10445500_PROG6212POE_Final.Helpers;
using CMCS_ST10445500_PROG6212POE_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//ST10445500 - PROG6212POE - Final Submission

//Controllers: AccountController

//.......................................o0oSTART OF FILEo0o........................................//

namespace CMCS_ST10445500_PROG6212POE_Final.Controllers
{
    public class AccountController : Controller
    {

        private readonly AppDbContext _db;

        public AccountController(AppDbContext db) => _db = db;

        public IActionResult Login() => View();

        //.........................................................................//

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                HttpContext.Session.SetUser(user.Id, user.Role, user.Name);

                return user.Role switch
                {
                    UserRole.Lecturer => RedirectToAction("Index", "Lecturer"),
                    UserRole.Coordinator => RedirectToAction("Index", "Coordinator"),
                    UserRole.Manager => RedirectToAction("Index", "Manager"),
                    UserRole.HR => RedirectToAction("Index", "HR"),
                    _ => RedirectToAction("Login")
                };
            }

            ViewBag.Error = "Invalid credentials";
            return View();
        }

        //.........................................................................//

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        //.........................................................................//
    }
}

//........................................o0oEND OF FILEo0o.........................................//
