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

        public static InlineKeyboardButton[][] CreateChooseEmployeeMessageKeyboard(IEnumerable<Employee> employees)
        {
            var employeeList = new InlineKeyboardButton[employees.Count() / 3 + 1][];
            var employeeArr = employees.ToList();

            for (int i = 0; i < employees.Count() / 3 + 1; i++)
            {
                if (i == employees.Count() / 3)
                {
                    employeeList[i] = new InlineKeyboardButton[employees.Count() - (employees.Count() / 3) * 3];
                    for (int j = 0; j < employees.Count() - (employees.Count() / 3) * 3; j++)
                    {
                        employeeList[i][j] = InlineKeyboardButton.WithCallbackData(employeeArr[i * 3 + j].Name, $"/chooseEmpMess {employeeArr[i * 3 + j].Id}");
                    }
                    break;
                }
                else
                {
                    employeeList[i] = new InlineKeyboardButton[3];
                }
                for (int j = 0; j < 3; j++)
                {
                    employeeList[i][j] = InlineKeyboardButton.WithCallbackData(employeeArr[i * 3 + j].Name, $"/chooseEmpMess {employeeArr[i * 3 + j].Id}");
                }
            }
            employeeArr.Clear();

            return employeeList;
        }

        public static InlineKeyboardButton[][] CreateChooseClienMessagetKeyboard(IEnumerable<Client> clients, long employeeId)
        {
            var clientList = new InlineKeyboardButton[clients.Count() / 3 + 1][];
            var clientsArr = clients.ToList();

            for (int i = 0; i < clients.Count() / 3 + 1; i++)
            {
                if (i == clients.Count() / 3)
                {
                    clientList[i] = new InlineKeyboardButton[clients.Count() - (clients.Count() / 3) * 3];
                    for (int j = 0; j < clients.Count() - (clients.Count() / 3) * 3; j++)
                    {
                        clientList[i][j] = InlineKeyboardButton.WithCallbackData(clientsArr[i * 3 + j].Name, $"/chooseMessClient {employeeId} {clientsArr[i * 3 + j].Id}");
                    }
                    break;
                }
                else
                {
                    clientList[i] = new InlineKeyboardButton[3];
                }
                for (int j = 0; j < 3; j++)
                {
                    clientList[i][j] = InlineKeyboardButton.WithCallbackData(clientsArr[i * 3 + j].Name, $"/chooseMessClient {employeeId} {clientsArr[i * 3 + j].Id}");
                }
            }
            clientsArr.Clear();

            return clientList;
        }

    }   
}
