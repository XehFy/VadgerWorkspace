using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure;
using VadgerWorkspace.Infrastructure.Keyboards;

namespace VadgerWorkspace.Domain.Commands.Admin.InstantReply
{
    public class GetClientLink : TelegramCommand
    {
        public override string Name => "Получить ссылку на клиента";

        public async override Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(context);

            var currentAdmin = employeeRepository.GetAllGlobalAdmins().FirstOrDefault(e => e.Id == message.Chat.Id);

            if (currentAdmin == null)
            {
                await adminBot.SendTextMessageAsync(message.Chat.Id, "У вас нет прав для получения ссылок", replyMarkup: KeyboardAdmin.Empty);
                return;
            }

            ClientRepository clientRepository = new ClientRepository(context);
            var clients = clientRepository.FindAll().Where(c => c.Town != null && (c.IsActive == true || c.IsActive == null)).OrderBy(c => c.LastOrder);

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
                var clikeyboard = KeyboardAdmin.CreateGetLinkKeyboard(clientWithService);
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
            var clikeyboardOther = KeyboardAdmin.CreateGetLinkKeyboard(clientOtherService);
            await adminBot.SendTextMessageAsync(message.Chat.Id, "Другие", replyMarkup: new InlineKeyboardMarkup(clikeyboardOther));

            //var clientsNotRegistred = clientRepository.FindAll().Where(c => c.Town == null);

            //var clikeyboardNR = KeyboardAdmin.CreateGetLinkKeyboard(clientsNotRegistred);
            //if (clientsNotRegistred.Any()) 
            //{
            //    await adminBot.SendTextMessageAsync(message.Chat.Id, "Эти клиенты нажали старт в боте, но не заказали услугу", replyMarkup: new InlineKeyboardMarkup(clikeyboardNR));
            //}

        }

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
    }
}
