using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace VadgerWorkspace.Domain.UpdateHandlers
{
    internal interface IUpdateHandler
    {
        public Task Handle(ITelegramBotClient client, Update update, CancellationToken cancellationToken);
    }
}
