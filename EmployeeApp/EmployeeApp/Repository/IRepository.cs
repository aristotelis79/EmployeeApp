using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmployeeApp.Data.Entities;

namespace EmployeeApp.Repository
{
    /// <summary>
    /// Represents an entity repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public partial interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> Table { get; }

        Task<TEntity> GetById(object id, CancellationToken token = default);

        Task<int> InsertAsync(TEntity entity, CancellationToken token = default);

        Task<int> UpdateAsync(TEntity entity, CancellationToken token = default);

        Task<int> DeleteAsync(TEntity entity, CancellationToken token = default);
    }
}