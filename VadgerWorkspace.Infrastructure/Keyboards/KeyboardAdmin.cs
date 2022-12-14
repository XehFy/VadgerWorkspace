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
        public static ReplyKeyboardMarkup Management = new ReplyKeyboardMarkup(new[]
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

        public static InlineKeyboardButton[][] CreateChooseEmployeeKeyboard(IEnumerable<Employee> employees, Client client)
        {
            //var employeeList = new InlineKeyboardButton[employees.Count()];
            //int i = 0;
            //foreach (var employee in employees)
            //{
            //    employeeList[i] = InlineKeyboardButton.WithCallbackData(employee.Name.ToString(), $"/chooseEmp {employee.Id} {client.Id}");
            //    i++;
            //}
            //return employeeList;

            var employeeList = new InlineKeyboardButton[employees.Count() / 3 + 1][];
            var employeeArr = employees.ToList();

            for (int i = 0; i < employees.Count() / 3 + 1; i++)
            {
                if (i == employees.Count() / 3)
                {
                    employeeList[i] = new InlineKeyboardButton[employees.Count() - (employees.Count() / 3) * 3];
                    for (int j = 0; j < employees.Count() - (employees.Count() / 3) * 3; j++)
                    {
                        employeeList[i][j] = InlineKeyboardButton.WithCallbackData(employeeArr[i * 3 + j].Name, $"/chooseEmp {employeeArr[i * 3 + j].Id} {client.Id}");
                    }
                    break;
                }
                else
                {
                    employeeList[i] = new InlineKeyboardButton[3];
                }
                for (int j = 0; j < 3; j++)
                {
                    employeeList[i][j] = InlineKeyboardButton.WithCallbackData(employeeArr[i * 3 + j].Name, $"/chooseEmp {employeeArr[i * 3 + j].Id} {client.Id}");
                }
            }
            employeeArr.Clear();

            return employeeList;
        }

        public static ReplyKeyboardRemove Empty = new ReplyKeyboardRemove();
    }
}
