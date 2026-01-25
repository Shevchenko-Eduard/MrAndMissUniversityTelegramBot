using System.Security.Cryptography.X509Certificates;
using Telegram.Bot.Types.ReplyMarkups;

namespace MrAndMissUniversity.Keyboards;

public static class Keyboard
{
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
}
