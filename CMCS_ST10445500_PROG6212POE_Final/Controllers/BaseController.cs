using CMCS_ST10445500_PROG6212POE_Final.Helpers;
using CMCS_ST10445500_PROG6212POE_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//ST10445500 - PROG6212POE - Final Submission

//Controllers: BaseController

//.......................................o0oSTART OF FILEo0o........................................//

namespace CMCS_ST10445500_PROG6212POE_Final.Controllers
{
    public class BaseController : Controller
    {
        protected readonly AppDbContext _db;
        public BaseController(AppDbContext db) => _db = db;

        //...............................................................//
        protected IActionResult RequireLogin()
        {
            if (HttpContext.Session.GetUserId() == null)
                return RedirectToAction("Login", "Account");
            return null!;
        }

        //...............................................................//

        protected IActionResult RequireRole(UserRole requiredRole)
        {
            var role = HttpContext.Session.GetUserRole();
            if (role != requiredRole)
                return RedirectToAction("Login", "Account");
            return null!;
        }

        //...............................................................//
    }
}

//........................................o0oEND OF FILEo0o.........................................//
