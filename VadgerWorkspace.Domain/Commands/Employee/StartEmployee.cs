using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure.Keyboards;
using Microsoft.EntityFrameworkCore;

namespace VadgerWorkspace.Domain.Commands.Employee
{
    internal class StartEmployee : TelegramCommand
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
            var mes = await botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Здесь будет указана вся инфа и инструкции по боту",
                replyMarkup: KeyboardEmployee.Menu);
        }
    }
}
