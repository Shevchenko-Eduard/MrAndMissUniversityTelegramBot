using Telegram.Bot;
using Telegram.Bot.Types;
using MrAndMissUniversity.DbUtils;
using MrAndMissUniversity.Keyboards;
using System.Runtime.CompilerServices;
using MrAndMissUniversityTelegramBot;

namespace MrAndMissUniversity;

public static class Registration
{
    public static readonly string textPleaseTryAgain = "🔄 Что-то пошло не так. Попробуйте ещё раз.";
    public static readonly string textRegistrationContinua = "➡️ Продолжите регистрацию.\n";
    public static async Task PleaseTryAgain(ITelegramBotClient botClient, Chat chat)
    {
        await PleaseTryAgain(botClient: botClient, chat: chat);
    }
    public static async Task PleaseTryAgain(ITelegramBotClient botClient, long chatId)
    {
        await botClient.SendMessage(
            chatId,
            textPleaseTryAgain);
    }
    public static async Task RegistrationProcess(
        ITelegramBotClient botClient, long chatId, long userId, short RegistrationStep, Message? message = null, PhotoSize? photo = null, Byte[]? imageBytes = null)
    {
        switch (RegistrationStep)
        {
            case 1:
                {
                    if (message is not null)
                    {
                        await Step1.Done(botClient, chatId, userId, message);
                    }
                    return;
                }
            case 2:
                {
                    if (message is not null)
                    {
                        await Step2.Done(botClient, chatId, userId, message);
                    }
                    return;
                }
            case 3:
                {
                    if (message is not null)
                    {
                        await Step3.Done(botClient, chatId, userId, message);
                    }
                    return;
                }
            case 4:
                {
                    if (photo is not null && imageBytes is not null && message is not null)
                    {
                        await Step4.Done(botClient, chatId, userId, photo, imageBytes, message);
                    }
                    else
                    {
                        await botClient.SendMessage(
                        chatId,
                        "📷 Пожалуйста, отправьте фотографию для завершения шага.");
                        // пользователь отправил текстовое сообщение вместо фото
                    }
                    return;
                }
            case 6:
                {
                    if (message is not null)
                    {
                        await Step6.Done(botClient, chatId, userId, message);
                    }
                    return;
                }
            case 7:
                {
                    if (message is not null)
                    {
                        await Step7.Done(botClient, chatId, userId, message);
                    }
                    return;
                }
            case -1:
                {
                    break;
                }
        }
    }
    public static async Task RegistrationContinua(
        ITelegramBotClient botClient, long chatId, short step, Message message)
    {
        switch (step)
        {
            case 0:
                {
                    await Step0.Done(botClient, chatId, chatId, message);
                    return;
                }
            case 1:
                {
                    await MessageRegistrationContinua(Step0.text);
                    await DataBaseMethods.UpdateDeleteMessage(chatId, message.Id + 1);
                    return;
                }
            case 2:
                {
                    await MessageRegistrationContinua(Step1.text);
                    await DataBaseMethods.UpdateDeleteMessage(chatId, message.Id + 1);
                    return;
                }
            case 3:
                {
                    await MessageRegistrationContinua(Step2.text);
                    await DataBaseMethods.UpdateDeleteMessage(chatId, message.Id + 1);
                    return;
                }
            case 4:
                {
                    await MessageRegistrationContinua(Step3.text);
                    await DataBaseMethods.UpdateDeleteMessage(chatId, message.Id + 1);
                    return;
                }
            case 5:
                {
                    await Step4.Message(botClient, chatId, textRegistrationContinua + Step4.text);
                    await DataBaseMethods.UpdateDeleteMessage(chatId, message.Id + 1);
                    return;
                }
            case 6:
                {
                    await MessageRegistrationContinua(Step5.text);
                    await DataBaseMethods.UpdateDeleteMessage(chatId, message.Id + 1);
                    return;
                }
            case 7:
                {
                    await MessageRegistrationContinua(Step6.text);
                    await DataBaseMethods.UpdateDeleteMessage(chatId, message.Id + 1);
                    return;
                }
            case -1:
                {
                    return;
                }
            default:
                {
                    throw new Exception();
                }
        }
        async Task MessageRegistrationContinua(string text)
        {
            await botClient.SendMessage(
                chatId,
                textRegistrationContinua + text);
        }
    }
    public static async Task RegistrationContinua(
        ITelegramBotClient botClient, Chat chat, User user, Message message)
    {

        await RegistrationContinua(botClient, chat.Id,
            await DataBaseMethods.GetRegistrationStep(user.Id), message);
    }

    public static class Step0
    {
        public static readonly string text = "✍️ Пожалуйста, укажите фамилию, имя и отчество полностью.";
        public static readonly short NumStep = 0;
        public static async Task Message(
            ITelegramBotClient botClient, Chat chat)
        {
            await Message(botClient, chat.Id);
        }
        public static async Task Message(
            ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendMessage(
                chatId,
                text);
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user, Message message)
        {
            await Done(botClient, chat, user, message);
        }
        public static async Task Done(
            ITelegramBotClient botClient, long chatId, long userId, Message message)
        {
            short step = await DataBaseMethods.GetRegistrationStep(userId);
            if (step == 0)
            {
                await DataBaseMethods.UpdateRegistrationStep(chatId, 1);
                await Message(botClient, chatId);
                await DataBaseMethods.UpdateDeleteMessage(userId, message.Id + 1);
            }
            else
            {
                await RegistrationContinua(botClient, chatId, step, message);
            }
        }
    }
    public static class Step1
    {
        public static readonly string text = "🎓 Укажите курс и группу в формате: [курс номер_группы]";
        public static readonly short NumStep = 1;
        public static async Task Message(
            ITelegramBotClient botClient, Chat chat)
        {
            await Message(botClient, chat.Id);
        }
        public static async Task Message(
            ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendMessage(
                chatId,
                text);
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user, Message message)
        {
            await Done(botClient: botClient, chatId: chat.Id, userId: user.Id, message: message);
            return;
        }
        public static async Task Done(
            ITelegramBotClient botClient, long chatId, long userId, Message message)
        {
            if (await message.Text!.IsFullName())
            {
                string[] arrayFullName = message.Text!.Split(' ');
                await DataBaseMethods.UpdateFullName(
                    userId,
                    lastName: arrayFullName[0],
                    firstName: arrayFullName[1],
                    patronymic: arrayFullName[2]);
                await DataBaseMethods.UpdateRegistrationStep(userId, 2);
                await Message(botClient, chatId);
                await botClient.DeleteMessages(chatId, [await DataBaseMethods.GetDeleteMessage(userId), message.Id]);
                await DataBaseMethods.UpdateDeleteMessage(userId, message.Id + 1);
                return;
            }
            await botClient.DeleteMessage(chatId, message.Id);
            await botClient.EditMessageText(chatId, 
                await DataBaseMethods.GetDeleteMessage(userId),
                Step0.text + " " + textPleaseTryAgain);
            return;
        }
    }
    public static class Step2
    {
        public static readonly string text = "📘 Напишите полное название вашей специальности.";
        public static readonly short NumStep = 2;
        public static async Task Message(
            ITelegramBotClient botClient, Chat chat)
        {
            await Message(botClient, chat.Id);
        }
        public static async Task Message(
            ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendMessage(
                chatId,
                text);
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user, Message message)
        {
            await Done(botClient: botClient, chatId: chat.Id, userId: user.Id, message: message);
            return;
        }
        public static async Task Done(
            ITelegramBotClient botClient, long chatId, long userId, Message message)
        {
            if (await message.Text!.IsYearAndGroup())
            {
                string[] arrayYearAndGroup = message.Text!.Split(' ');
                await DataBaseMethods.UpdateYearAndGroup(
                    userId,
                    year: short.Parse(arrayYearAndGroup[0]),
                    group: arrayYearAndGroup[1]);
                await DataBaseMethods.UpdateRegistrationStep(userId, 3);
                await Message(botClient, chatId);
                await botClient.DeleteMessages(chatId, [await DataBaseMethods.GetDeleteMessage(userId), message.Id]);
                await DataBaseMethods.UpdateDeleteMessage(userId, message.Id + 1);
                return;
            }
            await botClient.DeleteMessage(chatId, message.Id);
            await botClient.EditMessageText(chatId, 
                await DataBaseMethods.GetDeleteMessage(userId),
                Step1.text + " " + textPleaseTryAgain);
            return;
        }
    }
    public static class Step3
    {
        public static readonly string text = "📸 Прикрепите вашу фотографию (хорошего качества с соотношением сторон 3:4).";
        public static readonly short NumStep = 3;
        public static async Task Message(
            ITelegramBotClient botClient, Chat chat)
        {
            await Message(botClient, chat.Id);
        }
        public static async Task Message(
            ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendMessage(
                chatId,
                text);
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user, Message message)
        {
            await Done(botClient: botClient, chatId: chat.Id, userId: user.Id, message: message);
            return;
        }
        public static async Task Done(
            ITelegramBotClient botClient, long chatId, long userId, Message message)
        {
            if (message.Text!.Length <= 60)
            {
                await DataBaseMethods.UpdateNameOfSpecialty(
                    userId,
                    nameOfSpecialty: message.Text);
                await DataBaseMethods.UpdateRegistrationStep(userId, 4);
                await Message(botClient, chatId);
                await botClient.DeleteMessages(chatId, [await DataBaseMethods.GetDeleteMessage(userId), message.Id]);
                await DataBaseMethods.UpdateDeleteMessage(userId, message.Id + 1);
                return;
            }
            await botClient.DeleteMessage(chatId, message.Id);
            await botClient.EditMessageText(chatId, 
                await DataBaseMethods.GetDeleteMessage(userId),
                Step2.text + " " + textPleaseTryAgain);
            return;
        }
    }
    public static class Step4
    {
        public static readonly string text = "ℹ️ Вы можете рассказать немного о себе?";
        public static readonly short NumStep = 4;
        public static async Task Message(
            ITelegramBotClient botClient, Chat chat, string str)
        {
            await Message(botClient, chat.Id, str);
        }
        public static async Task Message(
            ITelegramBotClient botClient, long chatId, string str)
        {
            await botClient.SendMessage(
                chatId,
                str,
                replyMarkup: Keyboard.DichotomousSurvey);
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user, PhotoSize photo, Byte[] imageBytes, Message message)
        {
            await Done(botClient: botClient, chatId: chat.Id, userId: user.Id, photo: photo, imageBytes: imageBytes, message: message);
            return;
        }
        public static async Task Done(
            ITelegramBotClient botClient, long chatId, long userId, PhotoSize photo, Byte[] imageBytes, Message message)
        {
            if (photo.Height >= 1270 && photo.Width >= 950)
            {
                await DataBaseMethods.UpdatePhotograph(userId, imageBytes);
                await DataBaseMethods.UpdateRegistrationStep(userId, 5);
                await Message(botClient, chatId, text);
                await botClient.DeleteMessages(chatId, [await DataBaseMethods.GetDeleteMessage(userId), message.Id]);
                return;
            }
            await botClient.DeleteMessage(chatId, message.Id);
            await botClient.EditMessageText(chatId, 
                await DataBaseMethods.GetDeleteMessage(userId),
                Step3.text + " " + textPleaseTryAgain);
            return;
        }
    }
    public static class Step5
    {
        public static readonly string text = "🗣️ Напишите несколько слов о себе: интересы, навыки, увлечения.";
        public static readonly short NumStep = 5;
        public static async Task Message(
            ITelegramBotClient botClient, Chat chat)
        {
            await Message(botClient, chat.Id);
        }
        public static async Task Message(
            ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendMessage(
                chatId,
                text);
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user, Message message)
        {
            await Done(botClient, chat.Id, user.Id, message);
        }
        public static async Task Done(
            ITelegramBotClient botClient, long chatId, long userId, Message message)
        {
            await Message(botClient, chatId);
            await DataBaseMethods.UpdateRegistrationStep(userId, 6);
            await DataBaseMethods.UpdateDeleteMessage(userId, message.Id + 1);
        }
    }
    public static class Step6
    {
        public static readonly string text = "🎯 Поделитесь, почему вы решили принять участие.";
        public static readonly short NumStep = 6;
        public static async Task Message(
            ITelegramBotClient botClient, Chat chat)
        {
            await Message(botClient, chat.Id);
        }
        public static async Task Message(
            ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendMessage(
                chatId,
                text);
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user, Message message)
        {
            await Done(botClient: botClient, chatId: chat.Id, userId: user.Id, message: message);
            return;
        }
        public static async Task Done(
            ITelegramBotClient botClient, long chatId, long userId, Message message)
        {
            if (message.Text is null)
            {
                throw new Exception();
            }
            if (message.Text.Length < 512)
            {
                await DataBaseMethods.UpdateBriefIntroduction(userId, message.Text);
                await DataBaseMethods.UpdateRegistrationStep(userId, 7);
                await Message(botClient, chatId);
                await botClient.DeleteMessages(chatId, [await DataBaseMethods.GetDeleteMessage(userId), message.Id]);
                await DataBaseMethods.UpdateDeleteMessage(userId, message.Id + 1);
                return;
            }
            await botClient.DeleteMessage(chatId, message.Id);
            await botClient.EditMessageText(chatId, 
                await DataBaseMethods.GetDeleteMessage(userId),
                Step5.text + " " + textPleaseTryAgain);
            return;
        }
        public static async Task Skip(
            ITelegramBotClient botClient, Chat chat, User user)
        {
            await Skip(botClient, chat.Id, user.Id);
        }
        public static async Task Skip(
            ITelegramBotClient botClient, long chatId, long userId)
        {
            await Message(botClient, chatId);
            await DataBaseMethods.UpdateRegistrationStep(userId, 7);
        }
    }
    public static class Step7
    {
        public static readonly short NumStep = 6;
        public static async Task Message(
            ITelegramBotClient botClient, Chat chat, User user)
        {
            await Message(botClient: botClient, chatId: chat.Id, userId: user.Id);
        }
        public static async Task Message(
            ITelegramBotClient botClient, long chatId, long userId)
        {
            await Survey.SendMessage(botClient, chatId, userId);
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user, Message message)
        {
            await Done(botClient: botClient, chatId: chat.Id, userId: user.Id, message: message);
            return;
        }
        public static async Task Done(
            ITelegramBotClient botClient, long chatId, long userId, Message message)
        {
            if (message.Text is null)
            {
                throw new Exception();
            }
            if (message.Text.Length < 512)
            {
                await DataBaseMethods.UpdateReason(userId, message.Text);
                await DataBaseMethods.UpdateRegistrationStep(userId, -1);
                await Message(botClient, chatId, userId);
                await botClient.DeleteMessages(chatId, [await DataBaseMethods.GetDeleteMessage(userId), message.Id]);
                return;
            }
            await botClient.DeleteMessage(chatId, message.Id);
            await botClient.EditMessageText(chatId, 
                await DataBaseMethods.GetDeleteMessage(userId),
                Step6.text + " " + textPleaseTryAgain);
            return;
        }
    }
}
