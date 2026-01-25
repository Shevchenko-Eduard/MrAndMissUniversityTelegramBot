using Microsoft.EntityFrameworkCore;

namespace MrAndMissUniversity.DbUtils;

public class DataBase: DbContext
{
    public static string DataBaseDirectory = "DbUtils";
    public static string DataBaseName = "Students.db";
    public static string pathToDataBaseDirectory = Path.Combine(
                Environment.CurrentDirectory, DataBaseDirectory);
    public static string pathToDataBase = Path.Combine(
                pathToDataBaseDirectory, DataBaseName);
    public DbSet<Student> Students { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Filename={pathToDataBase}");
    }
}
