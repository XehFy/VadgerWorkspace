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

namespace VadgerWorkspace.Domain.Commands.Client.InstantReply
{
    public class SelectServiceClient : TelegramCommand
    {
        public override string Name => @"/selectService";

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            ClientRepository clientRepository = new ClientRepository(context);
            var clientId = message.Chat.Id;


            var client = clientRepository.GetClientByIdSync(clientId);

            if (client == null) return false;


            return (client.Stage == Data.Stages.SelectService);
        }

        public override async Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            ClientRepository clientRepository = new ClientRepository(context);
            var clientId = message.Chat.Id;

            var client = clientRepository.GetClientByIdSync(clientId);
            client.Service = message.Text;
            client.Stage = Data.Stages.SelectTown;
            clientRepository.Update(client);
            await clientRepository.SaveAsync();

            clientRepository.Dispose();

            string text = $"Теперь выберете ваш город или общину";
            var mes = await clientBot.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: KeyboardClient.SelectTown);

        }
    }
}
