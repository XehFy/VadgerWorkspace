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

                default:
                    break;
            }
        }

        public async Task ChooseEmp(string[] cbargs, CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            ClientRepository clientRepository = new ClientRepository(context);
            var employeeId = Convert.ToInt64(cbargs[1]);
            var clientId = Convert.ToInt64(cbargs[2]);

            var client = clientRepository.GetClientByIdSync(clientId);
            client.EmployeeId = employeeId;
            clientRepository.Update(client);
            var request = $"Вам назначен клиент:\n {client.Name}, {client.Town}\n{client.Service}";

            MessageRepository messageRepository = new MessageRepository(context);
            messageRepository.Create(new SavedMessage { ClientId = clientId, EmployeeId = employeeId, IsFromClient = false, Text = request });

            await clientRepository.SaveAsync();

            await employeeBot.SendTextMessageAsync(employeeId, request);
            // В перспективе тут надо обновить сообщения у админов шобы тупа убрать инлайн клаву

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
