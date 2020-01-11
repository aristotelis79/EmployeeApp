using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeApp.Data;
using EmployeeApp.Data.Entities;

namespace EmployeeApp.Controllers
{
    public class MvcEmployeeController : Controller
    {
        private readonly EmployeeDbContext _context;

        public MvcEmployeeController(EmployeeDbContext context)
        {
            _context = context;
        }

        // GET: MvcEmployee
        public async Task<IActionResult> Index()
        {
            var employeeDbContext = await _context.Employee.Include(e => e.EmpSupervisorNavigation)
                .Include(x=>x.EmployeeAttribute).ToListAsync();
            return View(employeeDbContext);
        }

        // GET: MvcEmployee/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.EmpSupervisorNavigation)
                .FirstOrDefaultAsync(m => m.EmpId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: MvcEmployee/Create
        public IActionResult Create()
        {
            ViewData["EmpSupervisor"] = new SelectList(_context.Employee, "EmpId", "EmpName");
            return View();
        }

        // POST: MvcEmployee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpId,EmpName,EmpDateOfHire,EmpSupervisor")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.EmpId = Guid.NewGuid();
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpSupervisor"] = new SelectList(_context.Employee, "EmpId", "EmpName", employee.EmpSupervisor);
            return View(employee);
        }

        // GET: MvcEmployee/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["EmpSupervisor"] = new SelectList(_context.Employee, "EmpId", "EmpName", employee.EmpSupervisor);
            return View(employee);
        }

        // POST: MvcEmployee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("EmpId,EmpName,EmpDateOfHire,EmpSupervisor")] Employee employee)
        {
            if (id != employee.EmpId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmpId))
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
            ViewData["EmpSupervisor"] = new SelectList(_context.Employee, "EmpId", "EmpName", employee.EmpSupervisor);
            return View(employee);
        }

        // GET: MvcEmployee/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.EmpSupervisorNavigation)
                .FirstOrDefaultAsync(m => m.EmpId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: MvcEmployee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(Guid id)
        {
            return _context.Employee.Any(e => e.EmpId == id);
        }
    }
}
