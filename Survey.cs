using Telegram.Bot;
using Telegram.Bot.Types;
using MrAndMissUniversity.DbUtils;
using MrAndMissUniversity.Keyboards;
using MrAndMissUniversity;

namespace MrAndMissUniversityTelegramBot;

public static class Survey
{
    
    public static async Task SendMessage(ITelegramBotClient botClient, Chat chat, User user)
    {
        await SendMessage(botClient, chat.Id, user.Id);
    }
    public static async Task SendMessage(ITelegramBotClient botClient, long chatId, long userId)
    {
        Student? student = await DataBaseMethods.GetUser(userId);
        if (student is null || student.Photograph is null)
        {
            return;
        }
        string survey = await Handler.ReadAllFile("Survey");
        using (Stream stream = new MemoryStream(student.Photograph))
        {
            await botClient.SendPhoto(
            chatId: chatId,
            photo: InputFile.FromStream(stream,
@$"{student.LastName}_{student.FirstName}_{student.Patronymic}_{student.Year}_{student.Group}_{student.NameOfSpecialty}.jpg"),
            caption: string.Format(survey,
                student.LastName,
                student.FirstName,
                student.Patronymic,
                student.Year,
                student.Group,
                student.NameOfSpecialty,
                student.BriefIntroduction ?? "отсутствует",
                student.Reason),
            replyMarkup: Keyboard.Edit
            );
        }
    }
    public static async Task EditProcess(
        ITelegramBotClient botClient, long chatId, long userId, short editColumn,
        Message? message, PhotoSize? photo = null, Byte[]? imageBytes = null)
    {
        if (await DataBaseMethods.IsRegistrationComplete(userId))
        {
            switch (editColumn)
            {
                case 1:
                    {
                        if (message is not null)
                        {
                            if (await message.Text!.IsFullName())
                            {
                                string[] arrayFullName = message.Text!.Split(' ');
                                await DataBaseMethods.UpdateFullName(
                                    userId,
                                    lastName: arrayFullName[0],
                                    firstName: arrayFullName[1],
                                    patronymic: arrayFullName[2]);
                                await DataBaseMethods.UpdateEditColumn(userId, -1);
                                await SendMessage(botClient, chatId, userId);
                                await botClient.DeleteMessages(chatId, [await DataBaseMethods.GetDeleteMessage(userId), message.Id]);
                            }
                            else
                            {
                                await botClient.DeleteMessage(chatId, message.Id);
                                await botClient.EditMessageText(chatId, message.Id - 1,
                                    Registration.Step0.text + " " + Registration.textPleaseTryAgain);
                            }
                        }
                        return;
                    }
                case 2:
                    {
                        if (message is not null)
                        {
                            if (await message.Text!.IsYearAndGroup())
                            {
                                string[] arrayYearAndGroup = message.Text!.Split(' ');
                                await DataBaseMethods.UpdateYearAndGroup(
                                    userId,
                                    year: short.Parse(arrayYearAndGroup[0]),
                                    group: arrayYearAndGroup[1]);
                                await DataBaseMethods.UpdateEditColumn(userId, -1);
                                await SendMessage(botClient, chatId, userId);
                                await botClient.DeleteMessages(chatId, [await DataBaseMethods.GetDeleteMessage(userId), message.Id]);
                            }
                            else
                            {
                                await botClient.DeleteMessage(chatId, message.Id);
                                await botClient.EditMessageText(chatId, message.Id - 1,
                                    Registration.Step1.text + " " + Registration.textPleaseTryAgain);
                            }
                        }
                        return;
                    }
                case 3:
                    {
                        if (message is not null)
                        {
                            if (message.Text!.Length <= 60)
                            {
                                await DataBaseMethods.UpdateNameOfSpecialty(userId, message.Text);
                                await DataBaseMethods.UpdateEditColumn(userId, -1);
                                await SendMessage(botClient, chatId, userId);
                                await botClient.DeleteMessages(chatId, [await DataBaseMethods.GetDeleteMessage(userId), message.Id]);
                            }
                            else
                            {
                                await botClient.DeleteMessage(chatId, message.Id);
                                await botClient.EditMessageText(chatId, message.Id - 1,
                                    Registration.Step2.text + " " + Registration.textPleaseTryAgain);
                            }
                        }
                        return;
                    }
                case 4:
                    {
                        if (photo is not null && imageBytes is not null && message is not null)
                        {
                            if (photo.Height >= 1270 && photo.Width >= 950)
                            {
                                await DataBaseMethods.UpdatePhotograph(userId, imageBytes);
                                await DataBaseMethods.UpdateEditColumn(userId, -1);
                                await SendMessage(botClient, chatId, userId);
                                await botClient.DeleteMessages(chatId, [await DataBaseMethods.GetDeleteMessage(userId), message.Id]);
                            }
                            else
                            {
                                await botClient.DeleteMessage(chatId, message.Id);
                                await botClient.EditMessageText(chatId, message.Id - 1,
                                    Registration.Step3.text + " " + Registration.textPleaseTryAgain);
                            }
                        }
                        return;
                    }
                case 5:
                    {
                        if (message is not null &&
                            message.Text is not null)
                        {
                            if (message.Text.Length < 512)
                            {
                                await DataBaseMethods.UpdateBriefIntroduction(userId, message.Text);
                                await DataBaseMethods.UpdateEditColumn(userId, -1);
                                await SendMessage(botClient, chatId, userId);
                                await botClient.DeleteMessages(chatId, [await DataBaseMethods.GetDeleteMessage(userId), message.Id]);
                            }
                            else
                            {
                                await botClient.DeleteMessage(chatId, message.Id);
                                await botClient.EditMessageText(chatId, message.Id - 1,
                                    Registration.Step4.text + " " + Registration.textPleaseTryAgain);
                            }
                        }
                        return;
                    }
                case 6:
                    {
                        if (message is not null &&
                            message.Text is not null)
                        {
                            if (message.Text.Length < 512)
                            {
                                await DataBaseMethods.UpdateReason(userId, message.Text);
                                await DataBaseMethods.UpdateEditColumn(userId, -1);
                                await SendMessage(botClient, chatId, userId);
                                await botClient.DeleteMessages(chatId, [await DataBaseMethods.GetDeleteMessage(userId), message.Id]);
                            }
                            else
                            {
                                await botClient.DeleteMessage(chatId, message.Id);
                                await botClient.EditMessageText(chatId, message.Id - 1,
                                    Registration.Step4.text + " " + Registration.textPleaseTryAgain);
                            }
                        }
                        return;
                    }
                case -1:
                    {
                        break;
                    }
            }
        }
        else
        {
            if (message is not null)
            {
                await Registration.RegistrationContinua(botClient, chatId: chatId, step:
                    await DataBaseMethods.GetRegistrationStep(Id: userId), message);
            }
        }
    }
}
