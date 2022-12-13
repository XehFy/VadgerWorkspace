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
using VadgerWorkspace.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace VadgerWorkspace.Domain.Commands.Client
{
    public class StartClient : TelegramCommand
    {
        public override string Name => @"/start";

        public override bool IsExecutionNeeded(Message message, ITelegramBotClient client)
        {
            if(message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, ITelegramBotClient botClient, DbContext context)
        {
            var client = new Client { }
            context.Add()
            string text = $"Здравствуйте, {message.Chat.FirstName} ! \r\n⚡️Компания «VadGer» оказывает комплексную помощь в решении задач и проблем при релокации в Montenegro 🇲🇪 \r\n💫Команда нашего шутер-агентства – это экспаты, проживающие в Черногории уже более 10 лет и владеющие ценными опытом. \r\n ✉️Выберете желаемую услугу";
            var mes = await botClient.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: KeyboardClient.SelectService);

        }
    }
}
