using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Newtonsoft.Json;
using MrAndMissUniversity.Keyboards;
using MrAndMissUniversity.DbUtils;
using static System.Console;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace MrAndMissUniversity;

public static class Handler
{
    static string pathToStartMessage = Path.Combine(Environment.CurrentDirectory, "StartMessage");
    static string startMessage = File.ReadAllText(pathToStartMessage);

    public static async Task<Task> ErrorHandlerAsync(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
    {
        WriteLine(JsonConvert.SerializeObject(exception));
        return Task.CompletedTask;
    }

    public static async Task UpdateHandlerAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        // эта переменная будет содержать в себе все связанное с сообщениями
                        Message? message = update.Message;
                        if (message is null)
                        {
                            return;
                        }
                        // From - это от кого пришло сообщение (или любой другой Update)
                        User? user = message.From;
                        if (user is null)
                        {
                            return;
                        }
                        // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
                        WriteLine(JsonConvert.SerializeObject(update));
                        // Chat - содержит всю информацию о чате
                        Chat chat = message.Chat;
                        switch (message.Type)
                        {
                            case MessageType.Text:
                                {
                                    if (message.Text == "/start")
                                    {
                                        Student? student = await DataBaseMethods.GetStudent(user.Id);
                                        if (student is null)
                                        {
                                            await botClient.SendMessage(
                                            chat.Id,
                                            startMessage,
                                            replyMarkup: Keyboard.Register());
                                            return;
                                        }
                                        else
                                        {
                                            await botClient.SendMessage(
                                            chat.Id,
                                            "Вы уже заполнили анкету.",
                                            replyMarkup: Keyboard.Register());
                                        }
                                    }
                                    if (message.Text == Keyboard.registerText)
                                    {
                                        
                                    }
                                    return;
                                }
                            // Добавил default , чтобы показать вам разницу типов Message
                            default:
                                {
                                    await botClient.SendMessage(
                                        chat.Id,
                                        "Используй только текст!");
                                    return;
                                }
                        }
                    }
                case UpdateType.CallbackQuery:
                    {
                        return;
                    }
            }
        }
        catch (Exception ex)
        {
            WriteLine(ex.ToString());
        }
    }
}