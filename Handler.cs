using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using MrAndMissUniversity.Keyboards;
using MrAndMissUniversity.DbUtils;
using static System.Console;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MrAndMissUniversity;

public static class Handler
{
    static string pathToStartMessage = Path.Combine(Environment.CurrentDirectory, "StartMessage");
    static string startMessage = File.ReadAllText(pathToStartMessage);
    static JsonSerializerOptions optionsJsonSerialize = new()
    {
        WriteIndented = true,  // Включаем красивое форматирование
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull  // Игнорируем null значения
    };

    public static async Task<Task> ErrorHandlerAsync(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
    {
        WriteLine(JsonSerializer.Serialize(exception, optionsJsonSerialize));
        return Task.CompletedTask;
    }

    public static async Task UpdateHandlerAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
            WriteLine(JsonSerializer.Serialize(update, optionsJsonSerialize));
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
                        // Chat - содержит всю информацию о чате
                        Chat chat = message.Chat;
                        switch (message.Type)
                        {
                            case MessageType.Text:
                                {
                                    switch (message.Text)
                                    {
                                        case "/start":
                                            {
                                                if (!await DataBaseMethods.ExistUser(user.Id))
                                                {
                                                    await DataBaseMethods.InitUser(user.Id);
                                                    await botClient.SendMessage(
                                                    chat.Id,
                                                    startMessage,
                                                    replyMarkup: Keyboard.Register);
                                                    return;
                                                }
                                                else if (await DataBaseMethods.IsRegistrationComplete(user.Id))
                                                {
                                                    await botClient.SendMessage(
                                                    chat.Id,
                                                    "Вы уже заполнили анкету.");
                                                    return;
                                                }
                                                else
                                                {
                                                    await botClient.SendMessage(
                                                    chat.Id,
                                                    startMessage,
                                                    replyMarkup: Keyboard.Register);
                                                    await Registration.RegistrationContinua(botClient, chat, user);
                                                    return;
                                                }
                                            }
                                        default:
                                            {
                                                short RegistrationStep = await DataBaseMethods.GetRegistrationStep(user.Id);
                                                if (!string.IsNullOrEmpty(message.Text))
                                                {
                                                    await Registration.RegistrationProcess(
                                                        RegistrationStep, botClient, chat, user, message);
                                                }
                                                return;
                                            }
                                    }
                                }
                            case MessageType.Photo:
                                {
                                    if (message.Photo is null)
                                    {
                                        return;
                                    }
                                    PhotoSize? photo = message.Photo.LastOrDefault(); // Берем самое крупное
                                    if (photo is null)
                                    {
                                        return;
                                    }
                                    string fileId = photo.FileId;
                                    TGFile fileInfo = await botClient.GetFile(fileId);
                                    string? filePath = fileInfo.FilePath;
                                    if (filePath is null)
                                    {
                                        return;
                                    }
                                    string url = $"https://api.telegram.org/file/bot{Program.Token}/{filePath}";
                                    Byte[] imageBytes;
                                    // Скачиваем файл
                                    using (HttpClient httpClient = new())
                                    {
                                        imageBytes = await httpClient.GetByteArrayAsync(url);
                                        // imageBytes содержит байты изображения
                                    }
                                    short RegistrationStep = await DataBaseMethods.GetRegistrationStep(user.Id);
                                    if (RegistrationStep == 4)
                                    {
                                        await Registration.Step4.Done(botClient, chat, user, photo, imageBytes);
                                    }
                                    return;
                                }
                        }
                        return;
                    }
                case UpdateType.CallbackQuery:
                    {
                        CallbackQuery? callbackQuery = update.CallbackQuery;
                        if (callbackQuery is null)
                        {
                            return;
                        }
                        User? user = callbackQuery.From;
                        if (user is null)
                        {
                            return;
                        }
                        switch (callbackQuery.Data)
                        {
                            case "/yes":
                                {
                                    short RegistrationStep = await DataBaseMethods.GetRegistrationStep(user.Id);
                                    if (RegistrationStep == 5)
                                    {
                                        await Registration.Step5.Done(botClient, user.Id, user.Id);
                                    }
                                    return;
                                }
                            case "/no":
                                {
                                    short RegistrationStep = await DataBaseMethods.GetRegistrationStep(user.Id);
                                    if (RegistrationStep == 5)
                                    {
                                        await Registration.Step6.Skip(botClient, user.Id, user.Id);
                                    }
                                    return;
                                }
                            case "/register":
                                {
                                    await Registration.Step0.Done(botClient, user.Id, user.Id);
                                    return;
                                }
                        }
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