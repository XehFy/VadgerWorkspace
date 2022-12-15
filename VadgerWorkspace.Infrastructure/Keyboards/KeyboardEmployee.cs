using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using VadgerWorkspace.Data.Entities;

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

        public static InlineKeyboardButton[][] CreateChooseClientKeyboard(IEnumerable<Client> clients, Employee employee)
        {
            var clientList = new InlineKeyboardButton[clients.Count()/3 + 1][];
            var clientsArr = clients.ToList();
            
            for (int i = 0; i < clients.Count() / 3 + 1; i++)
            {
                if (i == clients.Count() / 3)
                {
                    clientList[i] = new InlineKeyboardButton[clients.Count() - (clients.Count() / 3) * 3];
                    for (int j = 0; j < clients.Count() - (clients.Count() / 3) * 3; j++)
                    {
                        clientList[i][j] = InlineKeyboardButton.WithCallbackData(clientsArr[i * 3 + j].Name, $"/StartChat {employee.Id} {clientsArr[i * 3 + j].Id}");
                    }
                    break;
                }
                else 
                {
                    clientList[i] = new InlineKeyboardButton[3];
                }
                for (int j = 0; j < 3; j++)
                {
                    clientList[i][j] = InlineKeyboardButton.WithCallbackData(clientsArr[i * 3 + j].Name, $"/StartChat {employee.Id} {clientsArr[i * 3 + j].Id}");
                }
            }
            clientsArr.Clear();

            return clientList;
        }

        public static ReplyKeyboardRemove Empty = new ReplyKeyboardRemove();
    }
}
