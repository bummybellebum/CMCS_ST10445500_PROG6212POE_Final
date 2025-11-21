//ST10445500 - PROG6212POE - Final Submission

//Helpers: SessionHelper

//.......................................o0oSTART OF FILEo0o........................................//

using CMCS_ST10445500_PROG6212POE_Final.Models;

namespace CMCS_ST10445500_PROG6212POE_Final.Helpers
{
    public static class SessionHelper
    {
        //................................................................//
        public static void SetUser(this ISession session, int userId, UserRole role, string name)
        {
            session.SetInt32("UserId", userId);
            session.SetString("UserRole", role.ToString());
            session.SetString("UserName", name);
        }

        //................................................................//

        public static int? GetUserId(this ISession session) => session.GetInt32("UserId");

        //................................................................//

        public static UserRole? GetUserRole(this ISession session)
        {
            var roleStr = session.GetString("UserRole");
            if (roleStr == null) return null;
            return Enum.Parse<UserRole>(roleStr);
        }

        //................................................................//
    }
}

//........................................o0oEND OF FILEo0o.........................................//
