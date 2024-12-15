using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webOdev.Models;
using webOdev.NewFolder;

namespace webOdev.Controllers
{
    public class DoctorController : BaseController
    {
        private readonly AppDbContext _context;

        public DoctorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Doctor
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account"); // Admin olmayan kullanıcıları AccessDenied sayfasına yönlendir
            }

            return View(await _context.Doctors.ToListAsync());
        }

        // GET: Doctor/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account"); 
            }

            return View();
        }

        // POST: Doctor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Specialization,Email")] Doctor doctor)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account"); 
            }

            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // GET: Doctor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") 
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Specialization,Email")] Doctor doctor)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") 
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor); // Doktoru güncelle
                    await _context.SaveChangesAsync(); // Veritabanını güncelle
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Doctors.Any(e => e.Id == doctor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index)); // Başarılı güncelleme sonrası Index'e yönlendir
            }

            return View(doctor);
        }

        // GET: Doctor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account"); // Admin olmayan kullanıcıları AccessDenied sayfasına yönlendir
            }

            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }
        // POST: Doctor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") 
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var doctor = await _context.Doctors.FindAsync(id);       
                _context.Doctors.Remove(doctor); // Doktoru sil
                await _context.SaveChangesAsync(); // Veritabanını güncelle
            return RedirectToAction(nameof(Index)); // Başarılı silme sonrası Index'e yönlendir
        }
    }
}
