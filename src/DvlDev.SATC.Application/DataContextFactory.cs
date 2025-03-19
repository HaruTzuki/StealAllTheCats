using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DvlDev.SATC.Application;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlServer("Server=.\\SQL2019;Database=catdb;User Id=sa;Password=1309;TrustServerCertificate=True");

        return new DataContext(optionsBuilder.Options);
    }
}