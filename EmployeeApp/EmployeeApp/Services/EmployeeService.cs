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
            CheckForEmptyIds(employee);

            return (await _employeeRepository.InsertAsync(employee, token)
                                            .ConfigureAwait(false)) > 0
                                                                        ? employee
                                                                        : null;
        }

        public async Task<bool> UpdateAsync(Employee employee, IEnumerable<Guid> deleteAttributesIds = null, CancellationToken token = default)
        {
            CheckForEmptyIds(employee);

            var updateEmployes = _employeeRepository.UpdateAsync(employee,false, token);

            var deleteAttributes = DeleteAttributes(deleteAttributesIds, token);

            await Task.WhenAll(deleteAttributes, updateEmployes).ConfigureAwait(false);

            return await _employeeRepository.SaveChangesAsync(token).ConfigureAwait(false) > 0;
        }

        public async Task<bool> DeleteAsync(Employee employee, CancellationToken token = default)
        {
            var deleteEmployee = _employeeRepository.DeleteAsync(employee, false, token);

            var deleteAttributes = DeleteAttributes(employee.EmployeeAttribute.Select(s => s.EmpAttrAttributeId), token);

            await Task.WhenAll(deleteAttributes, deleteEmployee).ConfigureAwait(false);

            return await _employeeRepository.SaveChangesAsync(token).ConfigureAwait(false) > 0;
        }

        private Task<int> DeleteAttributes(IEnumerable<Guid> ids, CancellationToken token = default)
        {
            var employeeAttributes = _attributeRepository.Table.Where(x => ids.Contains(x.AttrId));

            return _attributeRepository.DeleteAsync(employeeAttributes, false, token);
        }

        public async Task<string> GetEmployeNameById(Guid emId, CancellationToken token = default)
        {
            return (await GetById(emId, token).ConfigureAwait(false))?.EmpName;
        }

        
        private static void CheckForEmptyIds(Employee employee)
        {
            if (employee.EmpId == Guid.Empty
                || employee.EmpSupervisor == Guid.Empty
                || employee.EmployeeAttribute.Any(x =>
                    x.EmpAttrAttributeId == Guid.Empty || x.EmpAttrEmployeeId == Guid.Empty)
                || employee.EmployeeAttribute.Any(x => x.EmpAttrAttribute.AttrId == Guid.Empty))
            {
                throw new ArgumentException("Not valid Id for employee or an Attribute");
            }
        }
    }
}