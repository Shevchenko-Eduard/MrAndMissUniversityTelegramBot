using Microsoft.EntityFrameworkCore;

namespace MrAndMissUniversity.DbUtils;

public class DataBase: DbContext
{
    public static string DataBaseName = "Students.db";

    public static string pathToDataBase = Path.Combine(
                Environment.CurrentDirectory, DataBaseName);
    public DbSet<Student> Students { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Filename={pathToDataBase}");
    }
}
