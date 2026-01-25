using System.Security.Cryptography.X509Certificates;
using Telegram.Bot.Types.ReplyMarkups;

namespace MrAndMissUniversity.Keyboards;

public static class Keyboard
{
    public static ReplyKeyboardMarkup Start =
    new ReplyKeyboardMarkup(new List<KeyboardButton>()
        {
            new KeyboardButton("/start"),
        })
    {
        ResizeKeyboard = true,
    };
    public static InlineKeyboardMarkup Register = new(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Регистрация", callbackData: "/register"),
            },
        });
    public static InlineKeyboardMarkup DichotomousSurvey = new(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Да", callbackData: "/yes"),
                InlineKeyboardButton.WithCallbackData(text: "Нет", callbackData: "/no"),
            },
        });
    public static InlineKeyboardMarkup Edit = new(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Отредактировать", callbackData: "/edit"),
            },
        });
    public static InlineKeyboardMarkup EditProfile = new(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "ФИО", callbackData: "/editFullName"),
                InlineKeyboardButton.WithCallbackData(text: "Курс и группу", callbackData: "/editYearAndGroup"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Специальность", callbackData: "/editNameOfSpecialty"),
                InlineKeyboardButton.WithCallbackData(text: "Фото", callbackData: "/editPhotograph"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Информацию о себе", callbackData: "/editBriefIntroduction"),
                InlineKeyboardButton.WithCallbackData(text: "Причину участия", callbackData: "/editReason"),
            },
        });
}
