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
    public static string ReadAllFile(params string[] path)
    {
        return File.ReadAllText(Path.Combine(
            Environment.CurrentDirectory, string.Join(
                Path.DirectorySeparatorChar, path)));
    }
    public static string Token = ReadAllFile("Text", "Token");
    // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
    private static ITelegramBotClient? bot;
    static async Task Main()
    {
        bot = new TelegramBotClient(Token);
        Directory.CreateDirectory(DataBase.DataBaseDirectory);
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
        WriteLine("Запущен бот " + bot.GetMe().Result.FirstName);
        ReadLine();
    }
}
