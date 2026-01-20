using Microsoft.EntityFrameworkCore;

namespace MrAndMissUniversity.DbUtils;

public class Database: DbContext
{
    public DbSet<Student> Students { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string DatabaseName = "Students.db";
        string path = Path.Combine(
                Environment.CurrentDirectory, DatabaseName);
        optionsBuilder.UseSqlite($"Filename={path}");
    }
}
