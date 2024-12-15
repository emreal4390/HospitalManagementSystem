using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webOdev.NewFolder;
using webOdev.Models;

public class AppointmentsController : Controller
{
    private readonly AppDbContext _context;

    public AppointmentsController(AppDbContext context)
    {
        _context = context;
    }

    // Alınan tüm randevuları listeleme
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        var appointments = await _context.Appointments.ToListAsync();

        // Doktor ve Asistan listelerini ViewBag ile gönderiyoruz
        ViewBag.Doctors = await _context.Doctors.ToListAsync();
        ViewBag.Assistants = await _context.Assistants.ToListAsync();

        return View(appointments);
    }


// Randevu alma ekranı
[HttpGet]
    public async Task<IActionResult> Create()
    {
        var doctors = await _context.Doctors.ToListAsync();
        var assistants = await _context.Assistants.ToListAsync();

        // Eğer doktor veya asistan listesi boşsa, bu durum kontrol edilmelidir
        if (doctors == null || !doctors.Any())
        {
            ModelState.AddModelError("", "No doctors available. Please add doctors first.");
        }

        if (assistants == null || !assistants.Any())
        {
            ModelState.AddModelError("", "No assistants available. Please add assistants first.");
        }

        ViewBag.Doctors = doctors;
        ViewBag.Assistants = assistants;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAjax([FromBody] Appointment model)
    {
        // Aynı zaman diliminde randevu kontrolü
        bool isTaken = await _context.Appointments
            .AnyAsync(a =>
                a.Date == model.Date &&
                a.Time == model.Time &&
                (a.DoctorId == model.DoctorId || a.AssistantId == model.AssistantId)
            );

        if (isTaken)
        {
            return Json(new { success = false, message = "This time slot is already taken. Please choose another time." });
        }

        // Yeni randevu kaydı
        if (ModelState.IsValid)
        {
            _context.Appointments.Add(model);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Appointment created successfully!" });
        }

        return Json(new { success = false, message = "An error occurred. Please try again." });
    }


}
