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
using VadgerWorkspace.Data.Entities;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure;
using VadgerWorkspace.Infrastructure.Keyboards;

namespace VadgerWorkspace.Domain.Commands.Admin.InstantReply
{
    internal class DeactivateMass : TelegramCommand
    {
        public override string Name => "Отключить старых клиентов";

        public async override Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(context);

            var currentAdmin = employeeRepository.GetAllGlobalAdmins().FirstOrDefault(e => e.Id == message.Chat.Id);

            if (currentAdmin == null)
            {
                await adminBot.SendTextMessageAsync(message.Chat.Id, "У вас нет прав", replyMarkup: KeyboardAdmin.Empty);
                return;
            }
            else
            {
                ClientRepository clientRepository = new ClientRepository(context);
                var clients = clientRepository.FindAll().Where(c => (c.IsActive == true || c.IsActive == null) && c.LastOrder != null);
                var messageRep = new MessageRepository(context);
                StringBuilder ToSend = new StringBuilder();
                int Range = 14;
                DateTime deactivateDate = DateTime.Now.AddDays(-Range);
                foreach (var client in clients)
                {
                    var lastMessage = messageRep.FindAll().Where(message => message.ClientId == client.Id).OrderBy(m => m.Time).LastOrDefault() ;
                    if(lastMessage != null)
                    {
                        if (lastMessage.Time < deactivateDate && client.Service != "Подбор и содержание недвижимости🏠")
                        {
                            client.IsActive = false;
                            clientRepository.Update(client);
                        }
                    }
                }
                clientRepository.SaveSync();
                var admins = employeeRepository.GetAllAdmins();
                foreach (var admin in admins)
                {
                    await adminBot.SendTextMessageAsync(admin.Id, $"Клиенты, не активные более {Range} дней были отключены. Вы можете включить необходимых самостоятельно");
                }
                
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
