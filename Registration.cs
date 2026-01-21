using Telegram.Bot;
using Telegram.Bot.Types;
using MrAndMissUniversity.DbUtils;
using MrAndMissUniversity.Keyboards;

namespace MrAndMissUniversity;

public class Registration
{
    public static async Task PleaseTryAgain(ITelegramBotClient botClient, Chat chat)
    {
        await botClient.SendMessage(
            chat.Id,
            "Повторите попытку.");
    }
    public static async Task RegistrationProcess(short RegistrationStep,
        ITelegramBotClient botClient, Chat chat, User user, Message message)
    {
        switch (RegistrationStep)
        {
            case 1:
                {
                    await Step1.Done(botClient, chat, user, message);
                    return;
                }
            case 2:
                {
                    await Step2.Done(botClient, chat, user, message);
                    return;
                }
            case 3:
                {
                    await Step3.Done(botClient, chat, user, message);
                    return;
                }
            case 4:
                {
                    await botClient.SendMessage(
                        chat.Id,
                        "Отправьте фотографию.");
                    return;
                }
            // case 5:
            //     {
            //         if (message.Text == "/yes")
            //         {
            //             await Step5.Done(botClient, chat, user);
            //         }
            //         else if (message.Text == "/no")
            //         {
            //             await Step6.Skip(botClient, chat, user);
            //         }
            //         return;
            //     }
            case 6:
                {
                    await Step6.Done(botClient, chat, user, message);
                    return;
                }
            case 7:
                {
                    await Step7.Done(botClient, chat, user, message);
                    return;
                }
            case -1:
                {
                    break;
                }
        }
    }
    public static async Task RegistrationContinua(
        ITelegramBotClient botClient, Chat chat, short step)
    {
        await botClient.SendMessage(
            chat.Id,
            "Продолжите регистрацию.");
        switch (step)
        {
            case 1:
                {
                    await Step0.Message(botClient, chat);
                    return;
                }
            case 2:
                {
                    await Step1.Message(botClient, chat);
                    return;
                }
            case 3:
                {
                    await Step2.Message(botClient, chat);
                    return;
                }
            case 4:
                {
                    await Step3.Message(botClient, chat);
                    return;
                }
            case 5:
                {
                    await Step4.Message(botClient, chat);
                    return;
                }
            case 6:
                {
                    await Step5.Message(botClient, chat);
                    return;
                }
            case 7:
                {
                    await Step6.Message(botClient, chat);
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
    }
    public static async Task RegistrationContinua(
        ITelegramBotClient botClient, Chat chat, User user)
    {

        await RegistrationContinua(botClient, chat,
            await DataBaseMethods.GetRegistrationStep(user.Id));
    }

    public class Step0
    {
        public static readonly short NumStep = 0;
        public static async Task Message(
            ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendMessage(
                chat.Id,
                "Введите ваше полное ФИО:");
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user)
        {
            short step = await DataBaseMethods.GetRegistrationStep(user.Id);
            if (step == 0)
            {
                await DataBaseMethods.UpdateRegistrationStep(chat.Id, 1);
                await Message(botClient, chat);
            }
            else
            {
                await RegistrationContinua(botClient, chat, step);
            }
        }
    }
    public class Step1
    {
        public static readonly short NumStep = 1;
        public static async Task Message(
            ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendMessage(
                chat.Id,
                "Введите курс и группу(Пример: 3 3831.9):");
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user, Message message)
        {
            if (await message.Text!.IsFullName())
            {
                string[] arrayFullName = message.Text!.Split(' ');
                await DataBaseMethods.UpdateFullName(
                    user.Id,
                    lastName: arrayFullName[0],
                    firstName: arrayFullName[1],
                    patronymic: arrayFullName[2]);
                await DataBaseMethods.UpdateRegistrationStep(user.Id, 2);
                await Message(botClient, chat);
                return;
            }
            await PleaseTryAgain(botClient, chat);
            await Step0.Message(botClient, chat);
            return;
        }
    }
    public class Step2
    {
        public static readonly short NumStep = 2;
        public static async Task Message(
            ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendMessage(
                chat.Id,
                "Введите название специальности:");
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user, Message message)
        {
            if (await message.Text!.IsYearAndGroup())
            {
                string[] arrayYearAndGroup = message.Text!.Split(' ');
                await DataBaseMethods.UpdateYearAndGroup(
                    user.Id,
                    year: short.Parse(arrayYearAndGroup[0]),
                    group: arrayYearAndGroup[1]);
                await DataBaseMethods.UpdateRegistrationStep(user.Id, 3);
                await Message(botClient, chat);
                return;
            }
            await PleaseTryAgain(botClient, chat);
            await Step1.Message(botClient, chat);
            return;
        }
    }
    public class Step3
    {
        public static readonly short NumStep = 3;
        public static async Task Message(
            ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendMessage(
                chat.Id,
                "Отправьте фотографию:");
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user, Message message)
        {
            if (message.Text!.Length <= 60)
            {
                await DataBaseMethods.UpdateNameOfSpecialty(
                    user.Id,
                    nameOfSpecialty: message.Text);
                await DataBaseMethods.UpdateRegistrationStep(user.Id, 4);
                await Message(botClient, chat);
                return;
            }
            await PleaseTryAgain(botClient, chat);
            await Step2.Message(botClient, chat);
            return;
        }
    }
    public class Step4
    {
        public static readonly short NumStep = 4;
        public static async Task Message(
            ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendMessage(
                chat.Id,
                "Хотите ли вы добавить информацию о себе?",
                replyMarkup: Keyboard.DichotomousSurvey);
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user, PhotoSize photo, Byte[] jpegImageBytes)
        {
            if (photo.Height >= 1270 && photo.Width >= 950)
            {
                await DataBaseMethods.UpdatePhotograph(user.Id, jpegImageBytes);
                await DataBaseMethods.UpdateRegistrationStep(user.Id, 5);
                await Message(botClient, chat);
                return;
            }
            await PleaseTryAgain(botClient, chat);
            await Step3.Message(botClient, chat);
            return;
        }
    }
    public class Step5
    {
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
                "Расскажи о себе:");
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user)
        {
            await Done(botClient, chat.Id, user.Id);
        }
        public static async Task Done(
            ITelegramBotClient botClient, long chatId, long userId)
        {
            await Message(botClient, chatId);
            await DataBaseMethods.UpdateRegistrationStep(userId, 6);
        }
    }
    public class Step6
    {
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
                "Расскажи о причине участия:");
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user, Message message)
        {
            if (message.Text is null)
            {
                throw new Exception();
            }
            if (message.Text.Length < 512)
            {
                await DataBaseMethods.UpdateBriefIntroduction(user.Id, message.Text);
                await DataBaseMethods.UpdateRegistrationStep(user.Id, 7);
                await Message(botClient, chat);
                return;
            }
            await PleaseTryAgain(botClient, chat);
            await Step5.Message(botClient, chat);
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
    public class Step7
    {
        public static readonly short NumStep = 6;
        public static async Task Message(
            ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendMessage(
                chat.Id,
                "Спасибо что заполнили анкету.");
        }
        public static async Task Done(
            ITelegramBotClient botClient, Chat chat, User user, Message message)
        {
            if (message.Text is null)
            {
                throw new Exception();
            }
            if (message.Text.Length < 512)
            {
                await DataBaseMethods.UpdateReason(user.Id, message.Text);
                await DataBaseMethods.UpdateRegistrationStep(user.Id, -1);
                await Message(botClient, chat);
                return;
            }
            await PleaseTryAgain(botClient, chat);
            await Step6.Message(botClient, chat);
            return;
        }
    }
}
