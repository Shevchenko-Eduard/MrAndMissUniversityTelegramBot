using System.Security.Cryptography.X509Certificates;
using Telegram.Bot.Types.ReplyMarkups;

namespace MrAndMissUniversity.Keyboards;

public class Keyboard
{
    public static readonly string registerText = "/reg";
    public static ReplyKeyboardMarkup Register()
    {
        return new ReplyKeyboardMarkup(new List<KeyboardButton>()
        {
            new KeyboardButton(registerText),
        })
        {
            ResizeKeyboard = true,
        };
    }
} 
