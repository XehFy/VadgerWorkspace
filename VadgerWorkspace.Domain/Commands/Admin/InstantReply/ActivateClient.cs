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
    internal class ActivateClient : TelegramCommand
    {
        public override string Name => "Включить клиента";

        public async override Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(context);

            var currentAdmin = employeeRepository.GetAllGlobalAdmins().FirstOrDefault(e => e.Id == message.Chat.Id);

            if (currentAdmin == null)
            {
                var currentLocalAdmin = employeeRepository.GetAllAdmins().FirstOrDefault(a => a.IsLocalAdmin == true && a.Id == message.Chat.Id);

                if (currentLocalAdmin == null)
                {
                    await adminBot.SendTextMessageAsync(message.Chat.Id, "Вы не являетесь администратором", replyMarkup: KeyboardAdmin.Empty);
                    return;
                }

                var town = currentLocalAdmin.Town;

                ClientRepository clientRepository = new ClientRepository(context);
                var clients = clientRepository.GetAllClientsWithTown(town).Where(c => c.IsActive == false).OrderBy(c => c.LastOrder);

                foreach(var service in KeyboardClient.Services)
                {
                    List<Data.Entities.Client> clientWithService = new List<Data.Entities.Client>();
                    foreach(var client in clients)
                    {
                        if (client.Service == service)
                        {
                            clientWithService.Add(client);
                        }
                    }
                    var clikeyboard = KeyboardAdmin.ActivateClient(clientWithService);
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
                var clikeyboardOther = KeyboardAdmin.ActivateClient(clientOtherService);
                await adminBot.SendTextMessageAsync(message.Chat.Id, "Другие", replyMarkup: new InlineKeyboardMarkup(clikeyboardOther));


                //await adminBot.SendTextMessageAsync(message.Chat.Id, "У вас нет прав", replyMarkup: KeyboardAdmin.Empty);
                return;
            }
            else
            {
                ClientRepository clientRepository = new ClientRepository(context);
                var clients = clientRepository.FindAll().Where(c => c.IsActive == false).OrderBy(c => c.LastOrder);

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
                    var clikeyboard = KeyboardAdmin.ActivateClient(clientWithService);
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
                var clikeyboardOther = KeyboardAdmin.ActivateClient(clientOtherService);
                await adminBot.SendTextMessageAsync(message.Chat.Id, "Другие", replyMarkup: new InlineKeyboardMarkup(clikeyboardOther));

            }
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
