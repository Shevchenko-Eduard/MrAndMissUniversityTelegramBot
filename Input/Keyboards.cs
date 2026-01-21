using System.Security.Cryptography.X509Certificates;
using Telegram.Bot.Types.ReplyMarkups;

namespace MrAndMissUniversity.Keyboards;

public static class Keyboard
{
    public static ReplyKeyboardMarkup Register =
    new ReplyKeyboardMarkup(new List<KeyboardButton>()
        {
            new KeyboardButton("/register"),
        })
    {
        ResizeKeyboard = true,
    };
    public static InlineKeyboardMarkup DichotomousSurvey = new(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Да", callbackData: "/yes"),
                InlineKeyboardButton.WithCallbackData(text: "Нет", callbackData: "/no"),
            },
        });
}
