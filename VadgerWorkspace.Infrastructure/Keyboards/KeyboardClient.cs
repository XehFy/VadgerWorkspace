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
            new KeyboardButton[] {"Услуги по переезду"},
            new KeyboardButton[] {"Оформление документов"},
            new KeyboardButton[] {"Транспорт"},
            new KeyboardButton[] {"Недвижимость"},
            new KeyboardButton[] {"Рабочие вопросы"},
            new KeyboardButton[] {"Услуги переводчика"},
            new KeyboardButton[] {"Другое"}
        })
        {
            ResizeKeyboard = true
        };

        public static ReplyKeyboardMarkup SelectTown = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] {"Бар"},
            new KeyboardButton[] {"Будва"},
            new KeyboardButton[] {"Тиват"},
            new KeyboardButton[] {"Улицинь"},
            new KeyboardButton[] {"Другие"}
        })
        {
            ResizeKeyboard = true
        };

        public static ReplyKeyboardMarkup Menu = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] {"Запись сейчас", "Запись в календарь"},
            new KeyboardButton[] {"Получить PDF дневника", "Прочитать записи за день"},
            new KeyboardButton[] {"Напомнить пароль"},
        })
        {
            ResizeKeyboard = true
        };

        public static ReplyKeyboardRemove Empty = new ReplyKeyboardRemove();

    }
}
