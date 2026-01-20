using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MrAndMissUniversity.DbUtils;

public class Student
{
    [Key]
    [Required]
    public int TelegramId { get; set; }
    [Required]
    [StringLength(20)]
    public string FirstName { get; set; } = null!; // имя 
    [Required]
    [StringLength(20)]
    public string LastName { get; set; } = null!; // фамилия
    [Required]
    [StringLength(20)]
    public string Patronymic { get; set; } = null!; // отчество
    [Required]
    public short Year { get; set; } // курс
    [Required]
    [StringLength(10)]
    public string Group { get; set; } = null!; // группа
    [Required]
    [StringLength(20)]
    public string NameOfSpecialty { get; set; } = null!; // название специальности
    [Required]
    [Column(TypeName = "image")]
    public Byte[] Photograph { get; set; } = null!; // фотография
    public string? BriefIntroduction { get; set; } // Рассказ о себе
    [Required]
    public string Reason { get; set; } = null!; // Причина участия
    public Student(
        int TelegramId,
        string FirstName,
        string LastName,
        string Patronymic,
        short Year,
        string Group,
        string NameOfSpecialty,
        Byte[] Photograph,
        string? BriefIntroduction,
        string Reason
    )
    {
        this.TelegramId = TelegramId;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Patronymic = Patronymic;
        this.Year = Year;
        this.Group = Group;
        this.NameOfSpecialty = NameOfSpecialty;
        this.Photograph = Photograph;
        this.BriefIntroduction = BriefIntroduction;
        this.Reason = Reason;
    }
    public Student() { }
}