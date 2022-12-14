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

namespace VadgerWorkspace.Domain.Commands.Client.Waiting
{
    public class WaitingClient : NoTelegramCommand
    {
        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            ClientRepository clientRepository = new ClientRepository(context);
            var clientId = message.Chat.Id;


            var client = clientRepository.GetClientByIdSync(clientId);

            if (client == null) return false;


            return client.Stage == Data.Stages.Waiting;
        }

        public override async Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {

            string text = $"Пожалуйста ожидайте ответа нашего сотрудника! \nСпасибо за понимание!";
            var mes = await clientBot.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: KeyboardClient.Empty);

        }
    }
}
