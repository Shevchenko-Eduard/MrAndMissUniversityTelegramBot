using Telegram.Bot;
using Telegram.Bot.Polling;
using static System.Console;
using MrAndMissUniversity.DbUtils;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MrAndMissUniversity;

class Program
{
    public static string Token = "<YourToken>";
    // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
    private static ITelegramBotClient bot = new TelegramBotClient(Token);
    static async Task Main()
    {
        WriteLine("Запущен бот " + bot.GetMe().Result.FirstName);
        using (DataBase db = new())
        {
            try
            {
                await db.Database.MigrateAsync();
            }
            catch (InvalidOperationException ex)
            {
                WriteLine(ex);
                WriteLine();
                WriteLine("Попробуйте пересоздать миграцию базы данных.");
            }
        }
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
