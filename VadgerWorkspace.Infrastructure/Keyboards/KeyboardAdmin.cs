using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using VadgerWorkspace.Data.Entities;

namespace VadgerWorkspace.Infrastructure.Keyboards
{
    public class KeyboardAdmin
    {
        public static ReplyKeyboardMarkup Mangagement = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] {"Посмотреть диалог клиента"},
            new KeyboardButton[] {"Посмотреть клиентов работника"},
            new KeyboardButton[] {"Список работников"}
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
            new KeyboardButton[] {"Подгорица"},
            new KeyboardButton[] {"Другие"}
        })
        {
            ResizeKeyboard = true
        };

        public static ReplyKeyboardMarkup Menu = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] {"Выбрать клиента"},
            new KeyboardButton[] {"Управление"}
        })
        {
            ResizeKeyboard = true
        };

        public static InlineKeyboardButton[] CreateChooseEmployeeKeyboard(IEnumerable<Employee> employees, Client client)
        {
            var employeeList = new InlineKeyboardButton[employees.Count()];
            int i = 0;
            foreach (var employee in employees)
            {
                employeeList[i] = InlineKeyboardButton.WithCallbackData(employee.Name.ToString(), $"/chooseEmp {employee.Id} {client.Id}");
                i++;
            }
            return employeeList;
        }

        public static ReplyKeyboardRemove Empty = new ReplyKeyboardRemove();
    }
}
