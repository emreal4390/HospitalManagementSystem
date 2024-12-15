using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    protected bool IsAdmin()
    {
        var role = HttpContext.Session.GetString("Role");
        return role == "Admin";
    }
}
