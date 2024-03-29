﻿using System;
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
                    await ChooseEmpManagement(cbargs, query, clientBot, employeeBot, adminBot, context);
                    break;

                case "/ChangeEmpl":
                    await ChangeEmpl(cbargs, query, clientBot, employeeBot, adminBot, context);
                    break;
                case "/ChangeEmployeeWithClient":
                    await ChangeEmployeeWithClient(cbargs, query, clientBot, employeeBot, adminBot, context);
                    break;
                case "/ApproveEmpl":
                    await ApproveEmpl(cbargs, query, clientBot, employeeBot, adminBot, context);
                    break;
                case "/DeactivateClient":
                    await DeactivateClient(cbargs, query, clientBot, employeeBot, adminBot, context);
                    break;
                case "/ActivateClient":
                    await ActivateClient(cbargs, query, clientBot, employeeBot, adminBot, context);
                    break;

                default:
                    break;
            }
        }

        public async Task ApproveEmpl(string[] cbargs, CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            var employeeId = Convert.ToInt64(cbargs[1]);
            var descision = cbargs[2];
            var adminId = query.From.Id;
            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var empl = await employeeRepository.GetEmployeeByIdAsync(employeeId);
            switch (descision)
            {
                case "0":
                    await employeeBot.SendTextMessageAsync(employeeId, "Вам отказано в получении прав сотрудника");
                    await adminBot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, $"Вы вы отказали {empl.Name}", replyMarkup: null);

                    break;
                case "1":
                    var employee = await employeeRepository.GetEmployeeByIdAsync(employeeId);
                    var admin = await employeeRepository.GetEmployeeByIdAsync(adminId);
                    employee.Town = admin.Town;
                    employee.IsVerified = true;
                    await employeeBot.SendTextMessageAsync(employeeId, $"Вы стали работником \n{admin.Town}");
                    await adminBot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, $"Вы назначили сотрудником {empl.Name}", replyMarkup: null);

                    var adminsGlob = employeeRepository.GetAllGlobalAdmins();
                    foreach (var adminG in adminsGlob)
                    {
                        await adminBot.SendTextMessageAsync(adminG.Id, $"{adminG.Name} \n подтвердил: {employee.Name} как сотрудника \n{employee.Town}");
                    }

                    employeeRepository.Update(employee);
                    await employeeRepository.SaveAsync();
                    break;
            }

            employeeRepository.Dispose();
        }

        public async Task ChangeEmployeeWithClient(string[] cbargs, CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            ClientRepository clientRepository = new ClientRepository(context);
            var employeeId = Convert.ToInt64(cbargs[1]);
            var clientId = Convert.ToInt64(cbargs[2]);

            var client = clientRepository.GetClientByIdSync(clientId);


            client.EmployeeId = employeeId;
            clientRepository.Update(client);
            var request = $"Вам переназначен клиент:\n {client.Name}, {client.Town}\n{client.Service}";

            EmployeeRepository employeeRepository = new EmployeeRepository(context);

            var emp = employeeRepository.GetEmployeeByIdSync(employeeId);
            var actor = employeeRepository.GetEmployeeByIdSync(query.Message.Chat.Id);
            var Admins = employeeRepository.GetAllGlobalAdmins();

            foreach (var adm in Admins)
            {
                try
                {
                    await adminBot.SendTextMessageAsync(adm.Id, $"{actor.Name} \nПереназначил сотруднику: {emp.Name}\n клиента: {client.Name}, {client.Town}\n{client.Service}");
                }
                catch { }
            }

            MessageRepository messageRepository = new MessageRepository(context);
            messageRepository.Create(new SavedMessage { ClientId = clientId, EmployeeId = employeeId, IsFromClient = false, Text = request });

            await clientRepository.SaveAsync();

            await employeeBot.SendTextMessageAsync(employeeId, request, replyMarkup: KeyboardEmployee.Menu);

            clientRepository.Dispose();
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
            await employeeRepository.SaveAsync();
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

            string EmpName;
            if (client.EmployeeId != null) 
            {
                EmployeeRepository employeeRepository = new EmployeeRepository(context);
                var employee = await employeeRepository.GetEmployeeByIdAsync((long)client.EmployeeId);
                if(employee == null) { return; }
                EmpName = $"Сотрудник: {employee.Name}"; 
            }
            else { EmpName = "Сотрудник не назначен"; }

            await adminBot.SendTextMessageAsync(query.Message.Chat.Id, $"Вы можете посмотреть ссылку в боте для клиентов\n{client.Name}\n@{client.Tag}\n{client.Town}\n{client.Service}\n{EmpName}", parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2);
            if ((client.Town != null) && (client.Service != null))
            {         
                await clientBot.SendTextMessageAsync(query.Message.Chat.Id, $"[{client.Name}](tg://user?id={client.Id})\n@{client.Tag}\n{client.Town}\n{client.Service}\n{EmpName}", parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2);
            }
            else { 
                await clientBot.SendTextMessageAsync(query.Message.Chat.Id, $"[{client.Name}](tg://user?id={client.Id})", parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2); 
            }

            clientRepository.Dispose();
        }

        public async Task DeactivateClient(string[] cbargs, CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            ClientRepository clientRepository = new ClientRepository(context);
            var clientId = Convert.ToInt64(cbargs[1]);
            var client = await clientRepository.GetClientByIdAsync(clientId);

            if (client.EmployeeId != null)
            {
                EmployeeRepository employeeRepository = new EmployeeRepository(context);
                var employee = await employeeRepository.GetEmployeeByIdAsync((long)client.EmployeeId);
                if (employee != null) {
                    await employeeBot.SendTextMessageAsync(employee.Id, $"клиент отключен\n {client.Name}\n{client.Town}\n{client.Service}");
                }
            }

            await adminBot.SendTextMessageAsync(query.Message.Chat.Id, $"клиент отключен\n {client.Name}\n{client.Town}\n{client.Service}");

            client.IsActive = false;
            clientRepository.Update(client);
            clientRepository.SaveSync();

            clientRepository.Dispose();
        }

        public async Task ActivateClient(string[] cbargs, CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            ClientRepository clientRepository = new ClientRepository(context);
            var clientId = Convert.ToInt64(cbargs[1]);
            var client = await clientRepository.GetClientByIdAsync(clientId);

            if (client.EmployeeId != null)
            {
                EmployeeRepository employeeRepository = new EmployeeRepository(context);
                var employee = await employeeRepository.GetEmployeeByIdAsync((long)client.EmployeeId);
                if (employee != null)
                {
                    await employeeBot.SendTextMessageAsync(employee.Id, $"клиент включен\n {client.Name}\n{client.Town}\n{client.Service}");

                }
            }

            await adminBot.SendTextMessageAsync(query.Message.Chat.Id, $"клиент включен\n {client.Name}\n{client.Town}\n{client.Service}");

            client.IsActive = true;
            clientRepository.Update(client);
            clientRepository.SaveSync();
            clientRepository.Dispose();
        }

        public async Task ChangeEmpl(string[] cbargs, CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            ClientRepository clientRepository = new ClientRepository(context);
            var clientId = Convert.ToInt64(cbargs[1]);

            var client = clientRepository.GetClientByIdSync(clientId);

            var employeeRepository = new EmployeeRepository(context);
            string empName="";
            if(client == null) { return; }
            if (client.EmployeeId != null)
            {
                var employee = await employeeRepository.GetEmployeeByIdAsync((long)client.EmployeeId);
                empName = $"Сотрудник: {employee.Name}";
            }

            var requestDesc = $"выберете сотрудника для клиента {client.Name}\n{client.Town}\n{client.Service}\n{empName} ";

            var employees = employeeRepository.FindAll().Where(e => e.IsVerified == true);
            var adminsGlob = employeeRepository.GetAllGlobalAdmins();
            var empKeyboard = KeyboardAdmin.CreateChangeEmployeeWithClientKeyboard(employees, client);
            //foreach (var admin in adminsGlob)
            //{
            //    await adminBot.SendTextMessageAsync(admin.Id, requestDesc, replyMarkup: new InlineKeyboardMarkup(empKeyboard));
            //}
            try
            {
                await adminBot.SendTextMessageAsync(query.From.Id, requestDesc, replyMarkup: new InlineKeyboardMarkup(empKeyboard));
            }
            catch (Exception ex) { }
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

                EmployeeRepository employeeRepository = new EmployeeRepository(context);

                var emp = employeeRepository.GetEmployeeByIdSync(employeeId);
                var actor = employeeRepository.GetEmployeeByIdSync(query.Message.Chat.Id);
                var Admins = employeeRepository.GetAllGlobalAdmins();

                foreach (var adm in Admins)
                {
                    await adminBot.SendTextMessageAsync(adm.Id, $"{actor.Name} \nназначил сотруднику: {emp.Name}\n клиента: {client.Name}, {client.Town}\n{client.Service}");
                }

                MessageRepository messageRepository = new MessageRepository(context);
                messageRepository.Create(new SavedMessage { ClientId = clientId, EmployeeId = employeeId, IsFromClient = false, Text = request });

                await clientRepository.SaveAsync();
                await adminBot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, $"Вы назначили: {emp.Name} на этот заказ", replyMarkup: null);

                await employeeBot.SendTextMessageAsync(employeeId, request, replyMarkup: KeyboardEmployee.Menu);
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
            var messages = await messageRepository.GetAllMessagesByClientId(clientId);

            int counter = 0;
            StringBuilder ToSend = new StringBuilder();
            foreach(SavedMessage mess in messages)
            {
                counter++;
                string name = "сотрудник";
                if ((bool)mess.IsFromClient)
                {
                    name = client.Name;
                }
                string newMess = $"{name}:\n{mess.Time}\n'{mess.Text}'\n\n";
                ToSend.Append(newMess);
                if (counter % 3 == 0)
                {
                    await adminBot.SendTextMessageAsync(query.Message.Chat.Id, ToSend.ToString());
                    ToSend.Clear();
                }
            }

            if (ToSend.Length > 0)
            {
                await adminBot.SendTextMessageAsync(query.Message.Chat.Id, ToSend.ToString());
            }

            // В перспективе тут надо обновить сообщения у админов шобы тупа убрать инлайн клаву

            clientRepository.Dispose();
        }

        public async Task ChooseMessEmp(string[] cbargs, CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            ClientRepository clientRepository = new ClientRepository(context);
            var employeeId = Convert.ToInt64(cbargs[1]);

            var clients = await clientRepository.GetAllClientsForEmployee(employeeId);


            foreach (var service in KeyboardClient.Services)
            {
                List<Data.Entities.Client> clientWithService = new List<Data.Entities.Client>();
                foreach (var client in clients)
                {
                    if (client.Service == service)
                    {
                        clientWithService.Add(client);
                    }
                }
                var clikeyboard = KeyboardAdmin.CreateChooseClienMessagetKeyboard(clientWithService, employeeId);
                await adminBot.SendTextMessageAsync(query.Message.Chat.Id, service, replyMarkup: new InlineKeyboardMarkup(clikeyboard));
            }
            List<Data.Entities.Client> clientOtherService = new List<Data.Entities.Client>();

            foreach (var client in clients)
            {
                if (!KeyboardClient.Services.Contains(client.Service))
                {
                    clientOtherService.Add(client);
                }
            }
            var clikeyboardOther = KeyboardAdmin.CreateChooseClienMessagetKeyboard(clientOtherService, employeeId);
            await adminBot.SendTextMessageAsync(query.Message.Chat.Id, "Другие", replyMarkup: new InlineKeyboardMarkup(clikeyboardOther));


            // В перспективе тут надо обновить сообщения у админов шобы тупа убрать инлайн клаву

            clientRepository.Dispose();
        }
    }
}
