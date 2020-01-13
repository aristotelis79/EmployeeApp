using System.Threading;
using System.Threading.Tasks;
using EmployeeApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Data
{
    public partial interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}