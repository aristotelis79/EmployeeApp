using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EmployeeApp.Data.Entities;
using EmployeeApp.Repository;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeeService(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public async Task<Employee> GetById(Guid id, CancellationToken token = default)
        {
            return await _employeeRepository.Table
                .Include(x=>x.EmpSupervisorNavigation)
                .Include(x => x.EmployeeAttribute)
                .ThenInclude(x => x.EmpAttrAttribute)
                .FirstOrDefaultAsync(x => x.EmpId == id, cancellationToken: token)
                .ConfigureAwait(false);
        }

        public async Task<List<Employee>> GetAll(CancellationToken token = default)
        {
            return await _employeeRepository.Table
                .Include(x => x.EmpSupervisorNavigation)
                .Include(x => x.EmployeeAttribute)
                .ThenInclude(x => x.EmpAttrAttribute)
                .ToListAsync(token).ConfigureAwait(false);
        }

        public async Task<Employee> InsertAsync(Employee employee,  CancellationToken token = default)
        {
            return (await _employeeRepository.InsertAsync(employee, token)
                                            .ConfigureAwait(false)) > 0
                                                                        ? employee
                                                                        : null;
        }


        public async Task<bool> UpdateAsync(Employee employee, CancellationToken token)
        {
            return (await _employeeRepository.UpdateAsync(employee, token)
                       .ConfigureAwait(false)) > 0;

        }

        public async Task<bool> DeleteAsync(Employee employee, CancellationToken token)
        {
            return (await _employeeRepository.DeleteAsync(employee, token)
                                                .ConfigureAwait(false)) > 0;
        }
    }
}