using Telegram.Bot;
using Telegram.Bot.Types;
using MrAndMissUniversity.DbUtils;

namespace MrAndMissUniversity;

public class Registration
{
    public static async Task PleaseTryAgain(ITelegramBotClient botClient, Chat chat)
    {
        await botClient.SendMessage(
            chat.Id,
            "Повторите попытку.");
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
        public static async Task Next(
            ITelegramBotClient botClient, Chat chat, Message message)
        {
            await DataBaseMethods.UpdateRegistrationStep(chat.Id, 1);
            await Message(botClient, chat);
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
        public static async Task Next(
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
        public static async Task Next(
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
        public static async Task Next(
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
                ":");
        }
        public static async Task Next(
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
}
