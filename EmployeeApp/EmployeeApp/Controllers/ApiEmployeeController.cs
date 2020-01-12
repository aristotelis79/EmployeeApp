using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeApp.Data.Entities;
using EmployeeApp.Models;
using EmployeeApp.Models.Mappers;
using EmployeeApp.Services;
using Microsoft.Extensions.Logging;

namespace EmployeeApp.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class ApiEmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<ApiEmployeeController> _logger;


        public ApiEmployeeController(IEmployeeService employeeService, ILogger<ApiEmployeeController> logger)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _logger = logger?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeViewModel>>> Get(CancellationToken token = default)
        {
            return  (await _employeeService.GetAll(token)
                                            .ConfigureAwait(false)).ToViewModel();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeViewModel>> Get(Guid id, CancellationToken token = default)
        {
            var employee = await _employeeService.GetById(id,token)
                                                    .ConfigureAwait(false);

            if (employee == null)
            {
                return NotFound();
            }

            return employee.ToViewModel();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, EmployeeViewModel model, CancellationToken token = default)
        {
            try
            {
                var entity = await _employeeService.GetById(id,token)
                    .ConfigureAwait(false);

                if(entity == null)
                    return new NotFoundResult();

                if (model.EmpId != id || !ModelState.IsValid) return Content("not valid");

                var updateModel = await TryUpdateModelAsync(entity,"",
                        c=>c.EmpName, 
                        c=> c.EmpDateOfHire,
                        c=> c.EmpSupervisor,
                        c => c.EmployeeAttribute)
                    .ConfigureAwait(false);

                if (updateModel)
                    await _employeeService.UpdateAsync(entity, token).ConfigureAwait(false);
                else
                    return Content("not success update");


                return Content("true");
            }
            catch(Exception e)
            {
                _logger.LogError("Can't edit employee", e);
                return Content("false");
            }
            //if (id != employee.EmpId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(employee).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmployeeExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeViewModel>> Create(EmployeeViewModel model)
        {
            try
            {
                var employee = await _employeeService.InsertAsync(model.ToEntity())
                    .ConfigureAwait(false);

                return employee.ToViewModel();
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
                    Content = "deleted",
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
    }
}
