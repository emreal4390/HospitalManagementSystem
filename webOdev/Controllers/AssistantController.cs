using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webOdev.Models;
using webOdev.NewFolder;

namespace webOdev.Controllers
{
    public class AssistantController : Controller
    {
        private readonly AppDbContext _context;

        public AssistantController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Assistant
        public async Task<IActionResult> Index()
        {
            // Admin rolü kontrolü
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account"); // Admin olmayan kullanıcıları AccessDenied sayfasına yönlendir
            }

            return View(await _context.Assistants.ToListAsync());
        }

        // GET: Assistant/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account"); // Admin olmayan kullanıcıları AccessDenied sayfasına yönlendir
            }

            return View();
        }

        // POST: Assistant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Surname,PhoneNumber,Email")] Assistant assistant)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account"); // Admin olmayan kullanıcıları AccessDenied sayfasına yönlendir
            }

            if (ModelState.IsValid)
            {
                _context.Add(assistant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(assistant);
        }

        // GET: Assistant/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account"); // Admin olmayan kullanıcıları AccessDenied sayfasına yönlendir
            }

            if (id == null)
            {
                return NotFound();
            }

            var assistant = await _context.Assistants.FindAsync(id);
            if (assistant == null)
            {
                return NotFound();
            }
            return View(assistant);
        }

        // POST: Assistant/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,PhoneNumber,Email")] Assistant assistant)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account"); 
            }

            if (id != assistant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assistant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Assistants.Any(e => e.Id == assistant.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(assistant);
        }

        // GET: Assistant/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account"); 
            }

            if (id == null)
            {
                return NotFound();
            }

            var assistant = await _context.Assistants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assistant == null)
            {
                return NotFound();
            }

            return View(assistant);
        }

        // POST: Assistant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account"); 
            }

            var assistant = await _context.Assistants.FindAsync(id);
            _context.Assistants.Remove(assistant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
