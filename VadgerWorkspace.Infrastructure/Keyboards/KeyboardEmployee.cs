﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace VadgerWorkspace.Infrastructure.Keyboards
{
    public class KeyboardEmployee
    {
        public static ReplyKeyboardMarkup SelectService = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] {"Выбрать клиента"},
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

        public static ReplyKeyboardMarkup SelectClient = new ReplyKeyboardMarkup(new[]
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
            new KeyboardButton[] {"Выбрать клиента"}
        })
        {
            ResizeKeyboard = true
        };

        public static ReplyKeyboardRemove Empty = new ReplyKeyboardRemove();
    }
}
