using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Microsoft.EntityFrameworkCore;

namespace VadgerWorkspace.Domain.Abstractions
{
    public interface INorTelegramCommand
    {
        Task Execute(Message message, ITelegramBotClient client, DbContext context);
        bool IsExecutionNeeded(Message message, ITelegramBotClient client);
    }
}
