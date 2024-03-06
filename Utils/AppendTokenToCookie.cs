namespace BookManager.Utils
{
    public class AppendTokenToCookie
    {
        public static void AppendToCookie(string token, HttpContext httpContext) 
        {
            httpContext.Response.Cookies.Append("token", token, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(120),
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            });
        }
    }
}
