using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MrAndMissUniversity.DbUtils
{
    public static class DataBaseMethods
    {
        public static async Task AddStudent(Student student)
        {
            using (Database db = new())
            {
                await db.Students.AddAsync(student);
                await db.SaveChangesAsync();
            }
        }
        public static async Task<Student?> GetStudent(long Id)
        {
            using (Database db = new())
            {
                Student? student = await db.Students.FirstOrDefaultAsync(s => s.TelegramId == Id);
                if (student is null)
                {
                    return null;
                }
                return student;
            }
        }
    }
}