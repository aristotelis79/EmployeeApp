using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmployeeApp.Data.Entities;
using EmployeeApp.Repository;
using Microsoft.EntityFrameworkCore;
using Attribute = EmployeeApp.Data.Entities.Attribute;

namespace EmployeeApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Attribute> _attributeRepository;

        public EmployeeService(IRepository<Employee> employeeRepository, IRepository<Attribute> attributeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _attributeRepository = attributeRepository ?? throw new ArgumentNullException(nameof(attributeRepository));
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
            var deleteEmployee = _employeeRepository.DeleteAsync(employee, false, token);

            var employeeAttributes = _attributeRepository.Table.Where(x =>
                employee.EmployeeAttribute.Select(s => s.EmpAttrAttributeId).Contains(x.AttrId));

            var deleteAttributes = _attributeRepository.DeleteAsync(employeeAttributes,false, token);

            await Task.WhenAll(deleteAttributes, deleteEmployee).ConfigureAwait(false);

            return await _employeeRepository.SaveChangesAsync(token).ConfigureAwait(false) > 0;
        }
    }
}