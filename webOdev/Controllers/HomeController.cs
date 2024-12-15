using Microsoft.AspNetCore.Mvc;
using webOdev.NewFolder;
using Microsoft.EntityFrameworkCore;
using webOdev.Services;
using webOdev.Models;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly EmailService _emailService;

    public HomeController(AppDbContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        // Session bilgilerini ViewData ile aktarýyoruz
        ViewData["Role"] = HttpContext.Session.GetString("Role");
        ViewData["Username"] = HttpContext.Session.GetString("Username");

        var schedules = _context.Schedules.ToList();
        return View(schedules);
    }

    // Departmanlarý Listele
    [HttpGet]
    public async Task<IActionResult> Departments()
    {
        var departments = await _context.Departments.ToListAsync(); // Veritabanýndan departmanlarý çek
        return PartialView("_Departments", departments); // _Departments.cshtml'e gönder
    }
    [HttpPost]
    // GET: Emergency Create View
    [HttpGet]
    
    public IActionResult CreateEmergency()
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            return RedirectToAction("AccessDenied", "Account");
        }
        return View(); // CreateEmergency.cshtml'i döndürür
    }

    [HttpPost]
    [Route("Home/CreateEmergency")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEmergency(EmergencyViewModel model)
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        if (ModelState.IsValid)
        {
            var assistantEmails = _context.Assistants.Select(a => a.Email).ToList();  // Asistan e-posta adreslerini çekiyoruz
            var doctorEmails = _context.Doctors.Select(d => d.Email).ToList();        // Doktor e-posta adreslerini çekiyoruz

            var allEmails = assistantEmails.Concat(doctorEmails).ToList();  // Asistan ve doktorlarý birleþtiriyoruz
                                                                            // E-posta içeriði
            string subject = model.Title;
            string body = model.Message;

            // E-posta gönderme iþlemi
            await _emailService.SendEmailAsync(allEmails, subject, body);
            // E-posta gönderme iþlemleri
            TempData["SuccessMessage"] = "Emergency notifications sent successfully!";
            return RedirectToAction("Index");
        }

        return View(model); // Model geçersizse ayný sayfayý döndürür
    }




}
