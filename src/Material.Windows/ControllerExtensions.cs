using System;
using System.Web;
using System.Web.Mvc;

namespace Material
{
    public static class ControllerExtensions
    {
        public static void AddUserIdCookie(
            this Controller instance, 
            string userId)
        {
            if (instance == null)
            {
                throw new NullReferenceException();
            }

            var cookie = new HttpCookie("userId");
            cookie.Values["userId"] = userId;
            instance.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
        }

        public static string GetUserIdFromCookie(this Controller instance)
        {
            if (instance == null)
            {
                throw new NullReferenceException();
            }

            return instance.Request.Cookies["userId"]?.Values["userId"];
        }
    }
}
