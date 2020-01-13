using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EmployeeApp.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeApp.Models;
using EmployeeApp.Models.Mappers;
using EmployeeApp.Services;
using Microsoft.Extensions.Logging;

namespace EmployeeApp.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class ApiEmployeeController : ControllerBase
    {
        private const string _route = "/api/employees/";

        private readonly IEmployeeService _employeeService;
        private readonly ILogger<ApiEmployeeController> _logger;


        public ApiEmployeeController(IEmployeeService employeeService, ILogger<ApiEmployeeController> logger)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _logger = logger?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken token = default)
        {
            return  Ok((await _employeeService.GetAll(token)
                                            .ConfigureAwait(false)).ToViewModel());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken token = default)
        {
            var employee = await _employeeService.GetById(id,token)
                                                    .ConfigureAwait(false);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee.ToViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model, CancellationToken token = default)
        {
            try
            {
                var employee = await _employeeService.InsertAsync(model.ToEntity(), token).ConfigureAwait(false);
                
                await GetSupervisorName(model, token);

                return Created($"{_route}{employee.EmpId}", employee.ToViewModel());
            }
        
            catch (Exception e)
            {
                _logger.LogError("Can't create employee", e);
                return new ContentResult
                {
                    Content = "Can't create employee",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, EmployeeViewModel model, CancellationToken token = default)
        {
            try
            {
                var employee = await _employeeService.GetById(id,token)
                    .ConfigureAwait(false);

                if(employee == null)
                    return new NotFoundResult();

                if (model.EmpId != id || !ModelState.IsValid) return Content("not valid");

                await _employeeService.UpdateAsync(employee.UpdateEmployeeEntity(model), token).ConfigureAwait(false);

                await GetSupervisorName(model, token);

                return Created($"{_route}{id}", model);
            }
            catch(Exception e)
            {
                _logger.LogError("Can't edit employee", e);
                return Content("false");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token = default)
        {
            try
            {
                var entity = await _employeeService.GetById(id,token)
                    .ConfigureAwait(false);

                if(entity == null) return new ContentResult
                {
                    Content = "Didn't find it",
                    StatusCode = StatusCodes.Status404NotFound
                };

                await _employeeService.DeleteAsync(entity, token)
                    .ConfigureAwait(false);

                return new ContentResult
                {
                    Content = $"Deleted employee with id:${id}",
                    StatusCode = StatusCodes.Status205ResetContent
                };
            }
            catch(Exception e)
            {
                _logger.LogError("Can't delete employee", e);
                return new ContentResult
                {
                    Content = "Can't delete employee",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        private async Task GetSupervisorName(EmployeeViewModel model, CancellationToken token)
        {
            if (model.EmpSupervisorId != null)
                model.EmpSupervisorName = await _employeeService.GetEmployeNameById((Guid) model.EmpSupervisorId, token)
                    .ConfigureAwait(false);
        }
    }
}
