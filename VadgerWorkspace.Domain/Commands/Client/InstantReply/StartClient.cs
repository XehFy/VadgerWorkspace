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
    public class StartClient : TelegramCommand
    {
        public override string Name => @"/start";

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
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
                    Stage = Data.Stages.starting,
                    LastOrder = DateTime.Now,
                    Link = message.Chat.LinkedChatId,
                    Tag = message.Chat.Username,
                });
                await clientRepository.SaveAsync();
            }
            else
            {
                client.Stage = Data.Stages.starting;
                client.LastOrder = DateTime.Now;
                clientRepository.Update(client);
            }
            clientRepository.Dispose();

            string response = $"Здравствуйте, {message.Chat.FirstName} ! \r\n⚡️Компания «VadGer» оказывает комплексную помощь в решении задач и проблем при релокации в Montenegro 🇲🇪 \r\n💫Команда нашего шутер-агентства – это экспаты, проживающие в Черногории уже более 10 лет и владеющие ценными опытом. \r\n";

            await clientBot.SendTextMessageAsync(clientId, response, replyMarkup: KeyboardClient.Menu);

        }
    }
}
