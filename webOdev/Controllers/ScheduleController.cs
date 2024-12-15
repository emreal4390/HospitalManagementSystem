using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webOdev.Models;
using webOdev.NewFolder;

public class ScheduleController : Controller
{
    private readonly AppDbContext _context;

    public ScheduleController(AppDbContext context)
    {
        _context = context;
    }

    // Home sayfasında sadece nöbetleri listeleme
    public async Task<IActionResult> Index()
    {
        return View(await _context.Schedules.ToListAsync());
    }

    // Admin için nöbet ekleme sayfası
    public IActionResult Create()
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        ViewData["Assistants"] = _context.Assistants.ToList();
        ViewData["Doctors"] = _context.Doctors.ToList();
        ViewData["Departments"] = _context.Departments.ToList();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,StartTime,EndTime,AssignedTo,Department")] Schedule schedule)
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        if (ModelState.IsValid)
        {
            _context.Add(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["Assistants"] = _context.Assistants.ToList();
        ViewData["Doctors"] = _context.Doctors.ToList();
        ViewData["Departments"] = _context.Departments.ToList();

        return View(schedule);
    }

    // GET: Schedule/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        if (id == null) return NotFound();

        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule == null) return NotFound();

        ViewData["Assistants"] = _context.Assistants.ToList();
        ViewData["Doctors"] = _context.Doctors.ToList();
        ViewData["Departments"] = _context.Departments.ToList();

        return View(schedule);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,StartTime,EndTime,AssignedTo,Department")] Schedule schedule)
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        if (id != schedule.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(schedule);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Schedules.Any(e => e.Id == schedule.Id))
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

        ViewData["Assistants"] = _context.Assistants.ToList();
        ViewData["Doctors"] = _context.Doctors.ToList();
        ViewData["Departments"] = _context.Departments.ToList();

        return View(schedule);
    }


    // GET: Schedule/Delete/5
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

        var schedule = await _context.Schedules.FirstOrDefaultAsync(m => m.Id == id);
        if (schedule == null)
        {
            return NotFound();
        }

        return View(schedule); // Views/Schedule/Delete.cshtml render edilir
    }



    // POST: Schedule/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule != null)
        {
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }



}
