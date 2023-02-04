using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace VadgerWorkspace.Infrastructure.Keyboards
{
    public class KeyboardClient
    {
        public static List<string> Services = new List<string>{
            "Получение ВНЖ🇲🇪",
            "Открытие ИП и фирмы💼",
            "Организация мероприятий🎉",
            "Подбор и содержание недвижимости🏠",
            "Покупка авто и получение прав🚗",
            "IT разработка💻",
            "Обмен криптовалют💰",
            "Обмен валют💶",
            "Индивидуальные вопросы❗️",
            "Услуги переводчика🈲",
        };
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

        public static List<string> Towns = new List<string>{
            "Бар",
            "Будва",
            "Тиват",
            "Котор",
            "Херцег-Нови",
            "Подгорица",
            "Ульцинь",
            "Другие",
        };

        public static ReplyKeyboardMarkup FAQ = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] {"Получение ВНЖ🇲🇪"},
            new KeyboardButton[] {"Открытие ИП и фирмы💼"},
            new KeyboardButton[] {"Подбор и содержание недвижимости🏠"},
            new KeyboardButton[] {"Покупка авто и получение прав🚗"},
            new KeyboardButton[] {"Обмен криптовалют💰"},
            new KeyboardButton[] {"Обмен валют💶"},
        })
        {
            ResizeKeyboard = true
        };

        public static ReplyKeyboardMarkup Menu = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] {"Часто задаваемые вопросы"},
            new KeyboardButton[] {"Заказать новую услугу"},
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

        public static ReplyKeyboardRemove Empty = new ReplyKeyboardRemove();

    }
}
