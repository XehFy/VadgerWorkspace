using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure.Keyboards;
using Microsoft.EntityFrameworkCore;

namespace VadgerWorkspace.Domain.Commands.Admin
{
    public class ManagementAdmin : TelegramCommand
    {
        public override string Name => @"Управление";

        public override bool IsExecutionNeeded(Message message, ITelegramBotClient client)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, ITelegramBotClient botClient, DbContext context)
        {
            var mes = await botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Mеню управления",
                replyMarkup: KeyboardAdmin.Mangagement);
        }
    }
}
