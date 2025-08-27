using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DBSCM.Context
{
    public class SCMDbContextFactory : IDesignTimeDbContextFactory<SCMDbContext>
    {
        public SCMDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SCMDbContext>();
            // Use your actual connection string or a development/test connection string
            optionsBuilder.UseSqlServer("Server=.;Database=YourDbName;Trusted_Connection=True;");
            return new SCMDbContext(optionsBuilder.Options);
        }
    }
}
