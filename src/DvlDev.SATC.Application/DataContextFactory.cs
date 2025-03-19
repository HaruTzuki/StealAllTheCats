using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DvlDev.SATC.Application;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=catsdb;User Id=sa;Password=Your_password123;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

        return new DataContext(optionsBuilder.Options);
    }
}