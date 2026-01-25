using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using MrAndMissUniversity.Keyboards;
using MrAndMissUniversity.DbUtils;
using static System.Console;
using System.Text.Json;
using System.Text.Json.Serialization;
using MrAndMissUniversityTelegramBot;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Telegram.Bot.Types.Payments;

namespace MrAndMissUniversity;

public static class Handler
{
    static Task<string> startMessage = ReadAllFile("Text", "StartMessage");
    public static async Task<List<int>> GetMessagesId(long IdUser, int actualMessagesId)
    {
        int startId = await DataBaseMethods.GetDeleteMessage(IdUser);
        List<int> messagesId = [startId];
        int difference = startId - actualMessagesId;
        for (int i = 0; i <= difference; ++i)
        {
            messagesId.Add(startId + 1);
        }
        return messagesId;
    }
    public async static Task<string> ReadAllFile(params string[] path)
    {
        return File.ReadAllText(Path.Combine(
            Environment.CurrentDirectory, string.Join(
                Path.DirectorySeparatorChar, path)));
    }
    static JsonSerializerOptions optionsJsonSerialize = new()
    {
        WriteIndented = true,  // Включаем красивое форматирование
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull  // Игнорируем null значения
    };

    public static async Task<Task> ErrorHandlerAsync(
        ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
    {
        WriteLine(JsonSerializer.Serialize(exception, optionsJsonSerialize));
        return Task.CompletedTask;
    }

    public static async Task UpdateHandlerAsync(
        ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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
                                                    await botClient.SendMessage(
                                                        chat.Id,
                                                        await startMessage,
                                                        replyMarkup: Keyboard.Register);
                                                    await DataBaseMethods.InitUser(user.Id);
                                                    await botClient.DeleteMessage(chat.Id, message.Id);
                                                    return;
                                                }
                                                else if (await DataBaseMethods.IsRegistrationComplete(user.Id))
                                                {
                                                    await Survey.SendMessage(botClient, chat, user);
                                                    await botClient.DeleteMessage(chat.Id, message.Id);
                                                    return;
                                                }
                                                else
                                                {
                                                    // await botClient.SendMessage(
                                                    // chat.Id,
                                                    // await startMessage,
                                                    // replyMarkup: Keyboard.Register);
                                                    await Registration.RegistrationContinua(botClient, chat, user, message);
                                                    await botClient.DeleteMessage(chat.Id, message.Id);
                                                    return;
                                                }
                                            }
                                        default:
                                            {
                                                Student? student = await DataBaseMethods.GetUser(user.Id);
                                                if (student is null || string.IsNullOrEmpty(message.Text))
                                                {
                                                    return;
                                                }
                                                if (student.RegistrationStep != -1)
                                                {
                                                    await Registration.RegistrationProcess(
                                                        RegistrationStep: student.RegistrationStep,
                                                        botClient: botClient, chatId:
                                                        chat.Id, userId: user.Id, message: message);
                                                }
                                                if (student.EditColumn != -1)
                                                {
                                                    await Survey.EditProcess(
                                                        editColumn: student.EditColumn,
                                                        botClient: botClient,
                                                        chatId: chat.Id,
                                                        userId: user.Id,
                                                        message: message);
                                                }
                                                return;
                                            }
                                    }
                                }
                            case MessageType.Photo:
                                {
                                    Student? student = await DataBaseMethods.GetUser(user.Id);
                                    if (student is null || message.Photo is null)
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

                                    if (student.RegistrationStep != -1)
                                    {
                                        await Registration.RegistrationProcess(
                                            RegistrationStep: student.RegistrationStep,
                                            botClient: botClient, chatId:
                                            chat.Id, userId: user.Id,
                                            photo: photo, imageBytes: imageBytes, 
                                            message: message);
                                    }
                                    if (student.EditColumn != -1)
                                    {
                                        await Survey.EditProcess(
                                            editColumn: student.EditColumn,
                                            botClient: botClient,
                                            userId: user.Id,
                                            chatId: user.Id,
                                            photo: photo,
                                            imageBytes: imageBytes,
                                            message: message);
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
                        Message? message = callbackQuery.Message;
                        if (message is null)
                        {
                            return;
                        }
                        Chat? chat = message.Chat;
                        User? user = callbackQuery.From;
                        if (user is null || chat is null)
                        {
                            return;
                        }
                        await botClient.AnswerCallbackQuery(
                            callbackQueryId: callbackQuery.Id,
                            text: null, // Можно передать текст для всплывающего уведомления
                            showAlert: false,
                            cancellationToken: cancellationToken);
                        switch (callbackQuery.Data)
                        {
                            case "/yes":
                                {
                                    short RegistrationStep = await DataBaseMethods.GetRegistrationStep(user.Id);
                                    if (RegistrationStep == 5)
                                    {
                                        await Registration.Step5.Done(botClient, chat.Id, user.Id, message);
                                    }
                                    await botClient.DeleteMessage(chat.Id, message.Id);
                                    return;
                                }
                            case "/no":
                                {
                                    short RegistrationStep = await DataBaseMethods.GetRegistrationStep(user.Id);
                                    if (RegistrationStep == 5)
                                    {
                                        await Registration.Step6.Skip(botClient, chat.Id, user.Id, message);
                                    }
                                    await botClient.DeleteMessage(chat.Id, message.Id);
                                    return;
                                }
                            case "/register":
                                {
                                    await Registration.Step0.Done(botClient, chat.Id, user.Id, message);
                                    await botClient.DeleteMessage(chat.Id, message.Id);
                                    return;
                                }
                            case "/edit":
                                {
                                    await botClient.SendMessage(
                                        chat.Id,
                                        "Что вы хотите отредактировать?",
                                        replyMarkup: Keyboard.EditProfile
                                    );
                                    await botClient.DeleteMessage(chat.Id, message.Id);
                                    return;
                                }
                            case "/editFullName":
                                {
                                    await Registration.Step0.Message(botClient, chat.Id);
                                    await botClient.DeleteMessage(chat.Id, message.Id);
                                    await DataBaseMethods.UpdateDeleteMessage(user.Id, message.Id + 1);
                                    await DataBaseMethods.UpdateEditColumn(user.Id, 1);
                                    return;
                                }
                            case "/editYearAndGroup":
                                {
                                    await Registration.Step1.Message(botClient, chat.Id);
                                    await botClient.DeleteMessage(chat.Id, message.Id);
                                    await DataBaseMethods.UpdateDeleteMessage(user.Id, message.Id + 1);
                                    await DataBaseMethods.UpdateEditColumn(user.Id, 2);
                                    return;
                                }
                            case "/editNameOfSpecialty":
                                {
                                    await Registration.Step2.Message(botClient, chat.Id);
                                    await botClient.DeleteMessage(chat.Id, message.Id);
                                    await DataBaseMethods.UpdateDeleteMessage(user.Id, message.Id + 1);
                                    await DataBaseMethods.UpdateEditColumn(user.Id, 3);
                                    return;
                                }
                            case "/editPhotograph":
                                {
                                    await Registration.Step3.Message(botClient, chat.Id);
                                    await botClient.DeleteMessage(chat.Id, message.Id);
                                    await DataBaseMethods.UpdateDeleteMessage(user.Id, message.Id + 1);
                                    await DataBaseMethods.UpdateEditColumn(user.Id, 4);
                                    return;
                                }
                            case "/editBriefIntroduction":
                                {
                                    await Registration.Step5.Message(botClient, chat.Id);
                                    await botClient.DeleteMessage(chat.Id, message.Id);
                                    await DataBaseMethods.UpdateDeleteMessage(user.Id, message.Id + 1);
                                    await DataBaseMethods.UpdateEditColumn(user.Id, 5);
                                    return;
                                }
                            case "/editReason":
                                {
                                    await Registration.Step6.Message(botClient, chat.Id);
                                    await botClient.DeleteMessage(chat.Id, message.Id);
                                    await DataBaseMethods.UpdateDeleteMessage(user.Id, message.Id + 1);
                                    await DataBaseMethods.UpdateEditColumn(user.Id, 6);
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