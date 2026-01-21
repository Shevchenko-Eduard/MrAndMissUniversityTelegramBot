using Microsoft.EntityFrameworkCore;

namespace MrAndMissUniversity.DbUtils
{
    public static class DataBaseMethods
    {
        /// <summary>
        /// Инициализирует пользователя в DB
        /// </summary>
        /// <param name="Id">Telegram Id пользователя</param>
        /// <returns></returns>
        public static async Task InitUser(long Id)
        {
            using (DataBase db = new())
            {
                Student student = new()
                {
                    TelegramId = Id,
                    RegistrationStep = 0,
                };
                await db.Students.AddAsync(student);
                await db.SaveChangesAsync();
            }
        }
        /// <summary>
        /// Получение данных пользователя по ID.
        /// </summary>
        /// <param name="Id">Telegram Id пользователя.</param>
        /// <returns></returns>
        public static async Task<Student?> GetUser(long Id)
        {
            using (DataBase db = new())
            {
                Student? student = await db.Students
                    .FirstOrDefaultAsync(s => s.TelegramId == Id);
                if (student is null)
                {
                    return null;
                }
                return student;
            }
        }
        /// <summary>
        /// Проверяет на наличие пользователя в BD
        /// </summary>
        /// <param name="Id">Telegram Id пользователя</param>
        /// <returns></returns>
        public static async Task<bool> ExistUser(long Id)
        {
            using (DataBase db = new())
            {
                return await db.Students.AnyAsync(s => s.TelegramId == Id);
            }
        }
        public static async Task UpdateRegistrationStep(long Id, short InputStep)
        {
            using (DataBase db = new())
            {
                Student? student = await db.Students
                    .FirstOrDefaultAsync(s => s.TelegramId == Id);
                if (student is null)
                {
                    throw new Exception();
                }
                student.RegistrationStep = InputStep;
                db.Students.Update(student);
                await db.SaveChangesAsync();
            }
        }
        public static async Task<short> GetRegistrationStep(long Id)
        {
            using (DataBase db = new())
            {
                Student? student = await db.Students
                    .FirstOrDefaultAsync(s => s.TelegramId == Id);
                if (student is null)
                {
                    await InitUser(Id);
                    return 0;
                }
                return student.RegistrationStep;
            }
        }
        public static async Task<bool> IsRegistrationComplete(long Id)
        {
            using (DataBase db = new())
            {
                Student? student = await db.Students
                    .FirstOrDefaultAsync(s => s.TelegramId == Id);
                if (student is null)
                {
                    return false;
                }
                else
                {
                    if (student.FirstName is null ||
                        student.LastName is null ||
                        student.Patronymic is null ||
                        student.Year is null ||
                        student.Group is null ||
                        student.NameOfSpecialty is null ||
                        student.Photograph is null ||
                        student.Reason is null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
        public static async Task UpdateFullName(
            long Id, string lastName, string firstName, string patronymic)
        {
            using (DataBase db = new())
            {
                Student? student = await db.Students
                    .FirstOrDefaultAsync(s => s.TelegramId == Id);
                if (student is null)
                {
                    throw new Exception();
                }
                student.LastName = lastName;
                student.FirstName = firstName;
                student.Patronymic = patronymic;
                db.Students.Update(student);
                await db.SaveChangesAsync();
            }
        }
        public static async Task UpdateYearAndGroup(
            long Id, short year, string group)
        {
            using (DataBase db = new())
            {
                Student? student = await db.Students
                    .FirstOrDefaultAsync(s => s.TelegramId == Id);
                if (student is null)
                {
                    throw new Exception();
                }
                student.Year = year;
                student.Group = group;
                db.Students.Update(student);
                await db.SaveChangesAsync();
            }
        }
        public static async Task UpdateNameOfSpecialty(
            long Id, string nameOfSpecialty)
        {
            using (DataBase db = new())
            {
                Student? student = await db.Students
                    .FirstOrDefaultAsync(s => s.TelegramId == Id);
                if (student is null)
                {
                    throw new Exception();
                }
                student.NameOfSpecialty = nameOfSpecialty;
                db.Students.Update(student);
                await db.SaveChangesAsync();
            }
        }
        public static async Task UpdatePhotograph(
            long Id, Byte[] photograph)
        {
            using (DataBase db = new())
            {
                Student? student = await db.Students
                    .FirstOrDefaultAsync(s => s.TelegramId == Id);
                if (student is null)
                {
                    throw new Exception();
                }
                student.Photograph = photograph;
                db.Students.Update(student);
                await db.SaveChangesAsync();
            }
        }
        public static async Task<Byte[]> GetPhotograph(
            long Id)
        {
            using (DataBase db = new())
            {
                Student? student = await db.Students
                    .FirstOrDefaultAsync(s => s.TelegramId == Id);
                if (student is null)
                {
                    throw new Exception();
                }
                if (student.Photograph is null)
                {
                    throw new Exception();
                }
                return student.Photograph;
            }
        }
        public static async Task UpdateBriefIntroduction(
            long Id, string briefIntroduction)
        {
            using (DataBase db = new())
            {
                Student? student = await db.Students
                    .FirstOrDefaultAsync(s => s.TelegramId == Id);
                if (student is null)
                {
                    throw new Exception();
                }
                student.BriefIntroduction = briefIntroduction;
                db.Students.Update(student);
                await db.SaveChangesAsync();
            }
        }
        public static async Task UpdateReason(
            long Id, string reason)
        {
            using (DataBase db = new())
            {
                Student? student = await db.Students
                    .FirstOrDefaultAsync(s => s.TelegramId == Id);
                if (student is null)
                {
                    throw new Exception();
                }
                student.Reason = reason;
                db.Students.Update(student);
                await db.SaveChangesAsync();
            }
        }
    }
}