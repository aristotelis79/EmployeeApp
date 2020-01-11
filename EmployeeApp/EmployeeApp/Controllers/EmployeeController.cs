using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmployeeApp.Data.Entities;
using EmployeeApp.Models;
using EmployeeApp.Models.Mappers;
using EmployeeApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace EmployeeApp.Controllers
{

    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;


        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token = default)
        {
            var employees = await GetAllEmployees(token);
            return View(employees.ToViewModel());
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid id, CancellationToken token = default)
        {
            var employee = await _employeeService.GetById(id, token)
                                                    .ConfigureAwait(false);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee.ToViewModel());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new EmployeeViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token = default)
        {
            var employee = await _employeeService.GetById(id,token)
                                                .ConfigureAwait(false);

            if(employee == null)
                return new NotFoundResult();

 
            ViewData["EmpSupervisor"] = new SelectList(await GetAllEmployees(token),"EmpId", "EmpName", employee.EmpSupervisor);

            return View(employee.ToViewModel());
        }

        private async Task<List<Employee>> GetAllEmployees(CancellationToken token)
        {
            return await _employeeService.GetAll(token)
                .ConfigureAwait(false);
        }

    }
}
