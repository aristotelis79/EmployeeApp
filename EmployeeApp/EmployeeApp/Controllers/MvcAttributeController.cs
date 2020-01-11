using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeApp.Data;
using EmployeeApp.Data.Entities;
using EmployeeApp.Repository;
using Attribute = EmployeeApp.Data.Entities.Attribute;

namespace EmployeeApp.Controllers
{
    public class MvcAttributeController : Controller
    {
        private readonly EmployeeDbContext _context;
        private readonly IRepository<Employee> _employeeRepository;

        public MvcAttributeController(EmployeeDbContext context, IRepository<Employee> employeeRepository)
        {
            _context = context;
            _employeeRepository = employeeRepository;
        }

        // GET: MvcAttribute
        public async Task<IActionResult> Index()
        {
            //var employeeRepository = _employeeRepository.Table.ToListAsync();
            //return View(await employeeRepository.ToListAsync()); 
            return View(await _context.Attribute.ToListAsync());
        }

        // GET: MvcAttribute/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attribute = await _context.Attribute
                .FirstOrDefaultAsync(m => m.AttrId == id);
            if (attribute == null)
            {
                return NotFound();
            }

            return View(attribute);
        }

        // GET: MvcAttribute/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MvcAttribute/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttrId,AttrName,AttrValue")] Attribute attribute)
        {
            if (ModelState.IsValid)
            {
                attribute.AttrId = Guid.NewGuid();
                _context.Add(attribute);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attribute);
        }

        // GET: MvcAttribute/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attribute = await _context.Attribute.FindAsync(id);
            if (attribute == null)
            {
                return NotFound();
            }
            return View(attribute);
        }

        // POST: MvcAttribute/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AttrId,AttrName,AttrValue")] Attribute attribute)
        {
            if (id != attribute.AttrId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attribute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttributeExists(attribute.AttrId))
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
            return View(attribute);
        }

        // GET: MvcAttribute/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attribute = await _context.Attribute
                .FirstOrDefaultAsync(m => m.AttrId == id);
            if (attribute == null)
            {
                return NotFound();
            }

            return View(attribute);
        }

        // POST: MvcAttribute/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var attribute = await _context.Attribute.FindAsync(id);
            _context.Attribute.Remove(attribute);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttributeExists(Guid id)
        {
            return _context.Attribute.Any(e => e.AttrId == id);
        }
    }
}
