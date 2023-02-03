using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure.Keyboards;
using Microsoft.EntityFrameworkCore;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Data.Entities;
using VadgerWorkspace.Infrastructure;
using Telegram.Bot.Types.ReplyMarkups;


namespace VadgerWorkspace.Domain.Commands.Admin.InstantReply
{
    internal class ChangeClientEmployee : TelegramCommand
    {
        public override string Name => "Изменить назначенного сотрудника";

        public async override Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(context);

            var currentAdmin = employeeRepository.GetAllGlobalAdmins().FirstOrDefault(e => e.Id == message.Chat.Id);

            if (currentAdmin == null)
            {
                await adminBot.SendTextMessageAsync(message.Chat.Id, "У вас нет прав", replyMarkup: KeyboardAdmin.Empty);
                return;
            }

            ClientRepository clientRepository = new ClientRepository(context);
            

            var clients = clientRepository.FindAll().Where(c => c.EmployeeId != null && (c.IsActive == true || c.IsActive == null)).OrderBy(c => c.LastOrder);

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
                var clikeyboard = KeyboardAdmin.CreateChangeEmplKeyboard(clientWithService);
                await adminBot.SendTextMessageAsync(message.Chat.Id, service, replyMarkup: new InlineKeyboardMarkup(clikeyboard));
            }
            List<Data.Entities.Client> clientOtherService = new List<Data.Entities.Client>();

            foreach (var client in clients)
            {
                if (!KeyboardClient.Services.Contains(client.Service))
                {
                    clientOtherService.Add(client);
                }
            }
            var clikeyboardOther = KeyboardAdmin.CreateChangeEmplKeyboard(clientOtherService);
            await adminBot.SendTextMessageAsync(message.Chat.Id, "Другие", replyMarkup: new InlineKeyboardMarkup(clikeyboardOther));

            var clientsNullEmp = clientRepository.FindAll().Where(c => c.EmployeeId == null && c.Town != null && (c.IsActive == true || c.IsActive == null)).OrderBy(c => c.LastOrder);

            if (clientsNullEmp.Any())
            {
                var clikeyboardNullEmp = KeyboardAdmin.CreateChangeEmplKeyboard(clientsNullEmp);
                await adminBot.SendTextMessageAsync(message.Chat.Id, "У этих клиентов НЕ назначен сотрудник", replyMarkup: new InlineKeyboardMarkup(clikeyboardNullEmp));

            }
            else
            {
                await adminBot.SendTextMessageAsync(message.Chat.Id, "Всем клиентам, закончившим регистрацию, назначен сотрудник\nОтличная работа", replyMarkup: KeyboardAdmin.Menu);

            }

        }

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
    }
}
