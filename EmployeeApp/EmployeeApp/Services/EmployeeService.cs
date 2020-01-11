using System;
using System.Threading;
using System.Threading.Tasks;
using EmployeeApp.Data.Entities;
using EmployeeApp.Repository;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Services
{
    public class EmployeeService
    {
        private readonly IRepository<Employee> _contactRepository;

        public EmployeeService(IRepository<Employee> contactRepository)
        {
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        }

        public async Task<Employee> GetById(Guid id, CancellationToken token = default)
        {
            return await _contactRepository.Table
                .Include(x => x.EmployeeAttribute)
                .Include(x => x.InverseEmpSupervisorNavigation)
                .FirstOrDefaultAsync(x => x.EmpId == id, cancellationToken: token)
                .ConfigureAwait(false);
        }

    }
}