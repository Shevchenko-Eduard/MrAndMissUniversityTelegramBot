using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static System.Console;

namespace MrAndMissUniversity;

class Program
{
    // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
    private static ITelegramBotClient bot = new TelegramBotClient("8362813400:AAEVyEL1jB3j_-Y0cWVs277jnx1OKtP4Gac");
    static async Task Main()
    {
        WriteLine("Запущен бот " + bot.GetMe().Result.FirstName);
        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken cancellationToken = cts.Token;
        ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, // разрешено получать все виды апдейтов
        };
        bot.StartReceiving(
            Handler.UpdateHandlerAsync,
            Handler.ErrorHandlerAsync,
            receiverOptions,
            cancellationToken
        );
        ReadLine();
    }
}
