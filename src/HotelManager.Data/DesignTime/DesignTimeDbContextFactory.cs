using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HotelManager.Data.DesignTime;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HotelDbContext>
{
    public HotelDbContext CreateDbContext(string[] args)
    {
        var databasePath = ResolveDatabasePath();

        var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
        optionsBuilder.UseSqlite($"Data Source={databasePath}");

        return new HotelDbContext(optionsBuilder.Options);
    }

    private static string ResolveDatabasePath()
    {
        var overridePath = Environment.GetEnvironmentVariable("HOTELMANAGER_DB_PATH");
        if (!string.IsNullOrWhiteSpace(overridePath))
        {
            return Path.GetFullPath(overridePath);
        }

        var solutionRoot = FindSolutionRoot(Directory.GetCurrentDirectory())
            ?? FindSolutionRoot(AppContext.BaseDirectory);

        if (solutionRoot is null)
        {
            throw new InvalidOperationException(
                "Could not locate HotelManager.sln. Set HOTELMANAGER_DB_PATH to specify the SQLite database path.");
        }

        return Path.Combine(solutionRoot, "hotel.db");
    }

    private static string? FindSolutionRoot(string startPath)
    {
        var directory = new DirectoryInfo(startPath);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "HotelManager.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        return null;
    }
}
