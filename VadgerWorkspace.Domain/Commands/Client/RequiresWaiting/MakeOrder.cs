using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure;
using VadgerWorkspace.Infrastructure.Keyboards;

namespace VadgerWorkspace.Domain.Commands.Client.RequiresWaiting
{
    internal class MakeOrder : TelegramCommand
    {
        public override string Name => "Заказать услугу";

        public async override Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            ClientRepository clientRepository = new ClientRepository(context);
            var clientId = message.Chat.Id;

            var client = await clientRepository.GetClientByIdAsync(clientId);

            if (client == null)
            {
                clientRepository.Create(new Data.Entities.Client
                {
                    Name = message.Chat.FirstName,
                    Id = message.Chat.Id,
                    Stage = Data.Stages.SelectService,
                    Link = message.Chat.LinkedChatId,
                });
                await clientRepository.SaveAsync();
            }
            else
            {
                if (client.Stage == Data.Stages.Chating)
                {
                    await clientBot.SendTextMessageAsync(clientId, "Пожалуйста, дождитесь ответа от нашего работника", replyMarkup: KeyboardClient.Empty);
                    return;
                }
                //ПОТОМ УБРАТЬ
                client.EmployeeId = 0;
                //УБРАТЬ НАХОЙ
                client.Stage = Data.Stages.SelectService;
                clientRepository.Update(client);
                await clientRepository.SaveAsync();

            }
            clientRepository.Dispose();

            string text = $"✉️Выберете желаемую услугу из предложенных или опишите запрос";
            var mes = await clientBot.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: KeyboardClient.SelectService);
        }

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
    }
}
