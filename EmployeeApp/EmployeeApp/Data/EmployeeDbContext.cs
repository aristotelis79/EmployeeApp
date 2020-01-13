using EmployeeApp.Data.Entities;
using EmployeeApp.Data.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Data
{
    public partial class EmployeeDbContext: DbContext, IDbContext
    {
        #region Fields

        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeeAttribute> EmployeeAttribute { get; set; }
        public DbSet<Attribute> Attribute { get; set; }

        #endregion

        #region ctor


        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
        }

        #endregion


        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeAttributeConfiguration());
            modelBuilder.ApplyConfiguration(new AttributeConfiguration());

            base.OnModelCreating(modelBuilder);

        }

        public DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        #endregion
    }
}