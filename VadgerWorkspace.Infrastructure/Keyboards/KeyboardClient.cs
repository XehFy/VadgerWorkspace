using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace VadgerWorkspace.Infrastructure.Keyboards
{
    public class KeyboardClient
    {
        public static ReplyKeyboardMarkup SelectService = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] {"Получение ВНЖ🇲🇪"},
            new KeyboardButton[] {"Открытие ИП и фирмы💼"},
            new KeyboardButton[] {"Организация мероприятий🎉"},
            new KeyboardButton[] {"Подбор и содержание недвижимости🏠"},
            new KeyboardButton[] {"Покупка авто и получение прав🚗"},
            new KeyboardButton[] {"IT разработка💻"},
            new KeyboardButton[] {"Обмен криптовалют💰"},
            new KeyboardButton[] {"Обмен валют💶"},
            new KeyboardButton[] {"Индивидуальные вопросы❗️"},
            new KeyboardButton[] {"Услуги переводчика🈲"},
        })
        {
            ResizeKeyboard = true
        };

        public static ReplyKeyboardMarkup SelectTown = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] {"Бар"},
            new KeyboardButton[] {"Будва"},
            new KeyboardButton[] {"Тиват"},
            new KeyboardButton[] {"Котор"},
            new KeyboardButton[] {"Херцег-Нови"},
            new KeyboardButton[] {"Подгорица"},
            new KeyboardButton[] {"Ульцинь"},
            new KeyboardButton[] {"Другие"},
        })
        {
            ResizeKeyboard = true
        };

        public static ReplyKeyboardMarkup Menu = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] {"Заказать новую услугу"},
        })
        {
            ResizeKeyboard = true
        };

        public static ReplyKeyboardRemove Empty = new ReplyKeyboardRemove();

    }
}
