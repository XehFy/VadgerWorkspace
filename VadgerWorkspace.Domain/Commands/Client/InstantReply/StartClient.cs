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

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            ClientRepository repo = new ClientRepository(context);
            Data.Entities.Client client = null;
            //var client = await repo.FindByCondition(o => o.Id == message.Chat.Id)
            if (client == null) {
                repo.Create(new Data.Entities.Client
                {
                    Name = message.Chat.FirstName,
                    Id = message.Chat.Id,
                    Stage = Data.Stages.starting
                });
                await repo.SaveAsync();
            }
            else
            {

            }

            repo.Dispose();

            //var chatId = message.Chat.Id;
            //var u = await userRepository.GetUserByIdAsync(chatId);
            //if (u != null)
            //{
            //    await botClient.SendTextMessageAsync(message.Chat.Id, "Вы уже зарегистрированы " + u.Email, replyMarkup: Keyboard.Menu);
            //}
            //else
            //{
            //    u = new Data.Entities.User
            //    {
            //        Id = chatId
            //    };
            //    userRepository.CreateUser(u);
            //    await userRepository.SaveAsync();
            string text = $"Здравствуйте, {message.Chat.FirstName} ! \r\n⚡️Компания «VadGer» оказывает комплексную помощь в решении задач и проблем при релокации в Montenegro 🇲🇪 \r\n💫Команда нашего шутер-агентства – это экспаты, проживающие в Черногории уже более 10 лет и владеющие ценными опытом. \r\n ✉️Выберете желаемую услугу";
            var mes = await clientBot.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: KeyboardClient.SelectService);
            //}

        }
    }
}
