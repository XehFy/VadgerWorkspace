using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VadgerWorkspace.Data;
using VadgerWorkspace.Data.Entities;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Infrastructure;
using VadgerWorkspace.Infrastructure.Keyboards;

namespace VadgerWorkspace.Domain.Commands.Admin
{
    public class OnCallbackQueryAdmin
    {
        public async Task CallbackQueryAdminHandle(CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            var cbargs = query.Data.Split(' ');
            switch (cbargs[0])
            {
                case "/chooseEmp":

                    await ChooseEmp(cbargs, query, clientBot, employeeBot, adminBot, context);

                    break;

                case "/chooseEmpMess":

                    await ChooseMessEmp(cbargs, query, clientBot, employeeBot, adminBot, context);

                    break;

                case "/chooseMessClient":

                    await ChooseMessClient(cbargs, query, clientBot, employeeBot, adminBot, context);

                    break;

                case "/GetClientLink":
                    await ChooseClientLink(cbargs, query, clientBot, employeeBot, adminBot, context);
                    
                    break;

                case "/MakeAdmin":
                    await MakeAdmin(cbargs, query, clientBot, employeeBot, adminBot, context);
                    break;

                case "/chooseEmpManagement":


                    break;

                default:
                    break;
            }
        }

        public async Task ChooseEmpManagement(string[] cbargs, CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var empId = Convert.ToInt64(cbargs[1]);
            var employee = await employeeRepository.GetEmployeeByIdAsync(empId);

            var descider = await employeeRepository.GetEmployeeByIdAsync(query.Message.Chat.Id);
            descider.ManagementId = employee.Id;
            descider.Stage = Stages.Management;
            employeeRepository.Update(descider);

            string role = "";
            if (employee.IsAdmin && employee.IsLocalAdmin) role = "Локальный админ";
            if (employee.IsAdmin && !employee.IsLocalAdmin) role = "Глобальный админ";
            if (!employee.IsAdmin && !employee.IsLocalAdmin) role = "Сотрудник";
            string town = employee.Town;
            if (town == null) town = "Город не назначен";

            string text = $"сотрудник {employee.Name}\nГорода: {town} \nРоль {role}";
            await adminBot.SendTextMessageAsync(query.Message.Chat.Id, text, replyMarkup: KeyboardAdmin.Management);
            employeeRepository.Dispose();
        }

        public async Task MakeAdmin(string[] cbargs, CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            var employeeId = Convert.ToInt64(cbargs[1]);
            var descision = cbargs[2];

            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            switch (descision)
            {
                case "0":
                    await adminBot.SendTextMessageAsync(employeeId, "Вам отказано в получении прав администратора");
                    break;
                case "1":
                    var employee = await employeeRepository.GetEmployeeByIdAsync(employeeId);
                    await adminBot.SendTextMessageAsync(employeeId, "Вы получили права глобального администратора");
                    employee.IsAdmin = true;
                    employee.IsLocalAdmin = false;
                    employeeRepository.Update(employee);
                    await employeeRepository.SaveAsync();
                    break;
                case "2":
                    var descider = await employeeRepository.GetEmployeeByIdAsync(query.Message.Chat.Id);
                    descider.Stage = Stages.SelectTown;
                    employeeRepository.Update(descider);
                    await employeeRepository.SaveAsync();
                    await adminBot.SendTextMessageAsync(query.Message.Chat.Id, "Введите города для этого админа\nПожалуйста, вводите названия городов так, как показано ниже\nБудва Бар Подгорица");
                    break;
            }
            employeeRepository.Dispose();
        }

        public async Task ChooseClientLink(string[] cbargs, CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            ClientRepository clientRepository = new ClientRepository(context);
            var clientId = Convert.ToInt64(cbargs[1]);
            var client  = await clientRepository.GetClientByIdAsync(clientId);
            await adminBot.SendTextMessageAsync(query.Message.Chat.Id, $"[{client.Name}](tg://user?id={client.Id})", parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2);
            clientRepository.Dispose();
        }

        public async Task ChooseEmp(string[] cbargs, CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            ClientRepository clientRepository = new ClientRepository(context);
            var employeeId = Convert.ToInt64(cbargs[1]);
            var clientId = Convert.ToInt64(cbargs[2]);

            var client = clientRepository.GetClientByIdSync(clientId);

            if (client.EmployeeId == 0 || client.EmployeeId == null)
            {
                client.EmployeeId = employeeId;
                clientRepository.Update(client);
                var request = $"Вам назначен клиент:\n {client.Name}, {client.Town}\n{client.Service}";

                MessageRepository messageRepository = new MessageRepository(context);
                messageRepository.Create(new SavedMessage { ClientId = clientId, EmployeeId = employeeId, IsFromClient = false, Text = request });

                await clientRepository.SaveAsync();

                await employeeBot.SendTextMessageAsync(employeeId, request);
            }
            else
            {
                EmployeeRepository employeeRepository = new EmployeeRepository(context);
                var employee = await employeeRepository.GetEmployeeByIdAsync((long)client.EmployeeId);
                await adminBot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId,$"{employee.Name} уже был назначен кем-то на этот заказ", replyMarkup: null);
            }
            // В перспективе тут надо обновить сообщения у админов шобы тупа убрать инлайн клаву
            //EmployeeRepository employeeRepository = new EmployeeRepository(context);
            //var admins = employeeRepository.GetAllAdmins();
            //var adminMadeChoose = admins.Where(a => a.Id == query.Message.Chat.Id).FirstOrDefault();
            //var choosenEmplooye = await employeeRepository.GetEmployeeByIdAsync(employeeId);
            //foreach (var admin in admins)
            //{
            //    await adminBot.EditMessageTextAsync(admin.Id, query.Message.MessageId, $"Админ {adminMadeChoose.Name}, назначил работника {choosenEmplooye.Name} на этот заказ", replyMarkup: null);
            //}

            clientRepository.Dispose();
        }

        public async Task ChooseMessClient(string[] cbargs, CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            var employeeId = Convert.ToInt64(cbargs[1]);

            ClientRepository clientRepository = new ClientRepository(context);
            var clientId = Convert.ToInt64(cbargs[2]);
            var client = clientRepository.GetClientByIdSync(clientId);

            MessageRepository messageRepository = new MessageRepository(context);
            var messagesFromClient = await messageRepository.GetAllMessagesByClientId(clientId);
            var messages = messagesFromClient.Where(client => client.EmployeeId == employeeId);

            StringBuilder ToSend = new StringBuilder();
            foreach(SavedMessage mess in messages)
            {
                string name = "сотрудник";
                if ((bool)mess.IsFromClient)
                {
                    name = client.Name;
                }
                string newMess = $"{name}:\n'{mess.Text}'\n\n";
                ToSend.Append(newMess);
            }

            await adminBot.SendTextMessageAsync(query.Message.Chat.Id, ToSend.ToString());
            // В перспективе тут надо обновить сообщения у админов шобы тупа убрать инлайн клаву

            clientRepository.Dispose();
        }

        public async Task ChooseMessEmp(string[] cbargs, CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            ClientRepository clientRepository = new ClientRepository(context);
            var employeeId = Convert.ToInt64(cbargs[1]);

            var clients = await clientRepository.GetAllClientsForEmployee(employeeId);

            var keyBoard = KeyboardAdmin.CreateChooseClienMessagetKeyboard(clients, employeeId);

            await adminBot.SendTextMessageAsync(query.Message.Chat.Id, "выберете клиента:", replyMarkup: new InlineKeyboardMarkup(keyBoard));
            // В перспективе тут надо обновить сообщения у админов шобы тупа убрать инлайн клаву

            clientRepository.Dispose();
        }
    }
}
