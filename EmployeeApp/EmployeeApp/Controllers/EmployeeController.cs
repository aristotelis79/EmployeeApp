using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EmployeeApp.Data.Entities;
using EmployeeApp.Models;
using EmployeeApp.Models.Mappers;
using EmployeeApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var employees = await _employeeService.GetAll(token)
                                                    .ConfigureAwait(false);
            return View(employees.ToViewModel());
        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid id, CancellationToken token = default)
        {
            var employee = await _employeeService.GetById(id,token)
                                                    .ConfigureAwait(false);

            if(employee == null)
                return new NotFoundResult();

            return  ViewComponent("DetailsEmployee",new {model = employee.ToViewModel()});
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken token = default)
        {
            await LoadSelectListSupervisors(token: token).ConfigureAwait(false);

            return  ViewComponent("CreateEmployee",EmployeeViewModel.New());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token = default)
        {
            var employee = await _employeeService.GetById(id,token)
                                                .ConfigureAwait(false);

            if(employee == null)
                return new NotFoundResult();

            await LoadSelectListSupervisors(token: token).ConfigureAwait(false);

            return  ViewComponent("EditEmployee",new {model = employee.ToViewModel()});
        }

        private async Task LoadSelectListSupervisors(Guid? selected = null , CancellationToken token = default)
        {
            var employees = new List<Employee> {new Employee()};
            employees.AddRange( await _employeeService.GetAll(token).ConfigureAwait(false));
            
            ViewData["EmpSupervisor"] = new SelectList(employees, "EmpId", "EmpName", selected);

        }
    }
}
