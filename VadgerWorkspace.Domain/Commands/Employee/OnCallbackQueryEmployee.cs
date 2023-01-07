using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Microsoft.EntityFrameworkCore;
using VadgerWorkspace.Infrastructure;
using VadgerWorkspace.Data;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Data.Entities;
using VadgerWorkspace.Infrastructure.Keyboards;
using Telegram.Bot.Types.ReplyMarkups;

namespace VadgerWorkspace.Domain.Commands.Employee
{
    public class OnCallbackQueryEmployee
    {
        public async Task CallbackQueryEmployeeHandle(CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            var cbargs = query.Data.Split(' ');
            switch (cbargs[0])
            {
                case "/StartChat":
                    await StartChat(cbargs, query, clientBot, employeeBot, adminBot, context);
                    break;
                case "/ChooseMaster":
                    await ChooseMaster(cbargs, query, clientBot, employeeBot, adminBot, context);
                    break;
            }
        }
        public async Task StartChat(string[] cbargs, CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            var employeeId = Convert.ToInt64(cbargs[1]);
            var clientId = Convert.ToInt64(cbargs[2]);

            //await clientBot.SendTextMessageAsync(clientId, "Был открыт чат с нашим работником, все последующие сообщения будут поступать от него и записываться в нашу систему");
            
            ClientRepository clientRepository = new ClientRepository(context);
            var client = clientRepository.GetClientByIdSync(clientId);
            client.EmployeeId = employeeId;
            client.Stage = Stages.Chating;
            clientRepository.Update(client);

            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var employee = await employeeRepository.GetEmployeeByIdAsync(employeeId);
            employee.Stage = Stages.Chating;
            employee.ClientId = clientId;
            

            await employeeBot.SendTextMessageAsync(employeeId, $"Вы открыли чат с клиентом {client.Name}");//, дальнейшие сообщения будут направлены клиенту и записываться для контроля качества.

            MessageRepository messageRepository = new MessageRepository(context);
            var messagesFromClient = await messageRepository.GetAllMessagesByClientId(clientId);
            var messages = messagesFromClient.Where(client => client.EmployeeId == employeeId);

            StringBuilder ToSend = new StringBuilder();
            foreach (SavedMessage mess in messages)
            {
                string name = "сотрудник";
                if ((bool)mess.IsFromClient)
                {
                    name = client.Name;
                }
                string newMess = $"{name}:\n{mess.Time}\n'{mess.Text}'\n\n";
                ToSend.Append(newMess);
            }
            await employeeBot.SendTextMessageAsync(query.Message.Chat.Id, ToSend.ToString());
            employeeRepository.Update(employee);
            await clientRepository.SaveAsync();
            clientRepository.Dispose();
        }

        public async Task ChooseMaster(string[] cbargs, CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            var adminId = Convert.ToInt64(cbargs[1]);

            var Keyboard = KeyboardAdmin.VerifyEmployee(query.From.Id);

            await employeeBot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, $"Прошение отправлено", replyMarkup: null);


            //await clientBot.SendTextMessageAsync(clientId, "Был открыт чат с нашим работником, все последующие сообщения будут поступать от него и записываться в нашу систему");
            await adminBot.SendTextMessageAsync(adminId, $"{query.From.FirstName} хочет стать вашим сотрудником", replyMarkup: new InlineKeyboardMarkup(Keyboard));//, дальнейшие сообщения будут направлены клиенту и записываться для контроля качества.
            
        }



    }
}
