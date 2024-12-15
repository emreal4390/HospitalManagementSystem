using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webOdev.NewFolder;
using webOdev.Models;
using Microsoft.AspNetCore.Authorization;

namespace webOdev.Controllers;




public class DepartmentController : Controller
{
    private readonly AppDbContext _context;

    public DepartmentController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Department
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            return RedirectToAction("AccessDenied", "Account"); 
        }
        return View(await _context.Departments.ToListAsync());
    }

    // GET: Department/Create
    public IActionResult Create()
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            return RedirectToAction("AccessDenied", "Account");
        }
        return View();
    }

    // POST: Department/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description,BedCount")] Department department)
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            return RedirectToAction("AccessDenied", "Account"); 
        }

        if (ModelState.IsValid)
        {
            _context.Add(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(department);
    }

    // GET: Department/Edit/5
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

        var department = await _context.Departments.FindAsync(id);
        if (department == null)
        {
            return NotFound();
        }
        return View(department);
    }

    // POST: Department/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,BedCount")] Department department)
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            return RedirectToAction("AccessDenied", "Account"); 
        }

        if (id != department.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(department);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Departments.Any(e => e.Id == department.Id))
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
        return View(department);
    }

    // GET: Department/Delete/5
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

        var department = await _context.Departments
            .FirstOrDefaultAsync(m => m.Id == id);
        if (department == null)
        {
            return NotFound();
        }

        return View(department);
    }

    // POST: Department/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            return RedirectToAction("AccessDenied", "Account"); 
        }

        var department = await _context.Departments.FindAsync(id);
        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
