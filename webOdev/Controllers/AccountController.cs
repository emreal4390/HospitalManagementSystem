using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webOdev.NewFolder;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Account/Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // POST: Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string username, string password)
    {
        // Kullanıcıyı doğrulama
        var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

        if (user != null)
        {
            // Kullanıcı doğrulandıktan sonra session başlatıyoruz
            HttpContext.Session.SetString("Username", user.Username);  // Kullanıcı adını session'a kaydediyoruz
            HttpContext.Session.SetString("Role", user.Role);          // Kullanıcı rolünü session'a kaydediyoruz

            return RedirectToAction("Index", "Home"); // Giriş başarılıysa ana sayfaya yönlendir
        }

        ViewBag.ErrorMessage = "Invalid username or password.";
        return View();
    }

    // POST: Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        // Oturumu sonlandırma
        HttpContext.Session.Clear(); // Session temizleniyor, kullanıcı çıkışı yapılmış oluyor
        return RedirectToAction("Login", "Account"); // Login sayfasına yönlendir
    }
}
