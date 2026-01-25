using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MrAndMissUniversity.DbUtils;

public class Student
{
    [Key]
    [Required]
    public long TelegramId { get; set; }
    [StringLength(40)]
    public string? FirstName { get; set; } // имя 
    [StringLength(40)]
    public string? LastName { get; set; } // фамилия
    [StringLength(40)]
    public string? Patronymic { get; set; } // отчество                         1
    public short? Year { get; set; } // курс
    [StringLength(10)]
    public string? Group { get; set; } // группа                                2
    [StringLength(60)]
    public string? NameOfSpecialty { get; set; } // название специальности      3
    public Byte[]? Photograph { get; set; } // фотография                       4
    public string? BriefIntroduction { get; set; } // Рассказ о себе            5
    public string? Reason { get; set; } // Причина участия                      6
    [DefaultValue(0)]
    public short RegistrationStep { get; set; } // Шаг регистрации 
    [DefaultValue(-1)]
    public short EditColumn { get; set; } // Какая колонка редактируется
    [DefaultValue(1)]
    public int StartDeleteMessage { get; set; } // Какая колонка редактируется
}