using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Services;
using Microsoft.EntityFrameworkCore;
using VadgerWorkspace.Data;

namespace VadgerWorkspace.Domain.UpdateHandlers
{
    public class AdminUpdateHandler
    {
        DbContext context = new VadgerContext();
        public async Task Handle(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            AdminCommandService commandService = new AdminCommandService();
            // NoCommandService noCommandService = (NoCommandService)_noCommandService;

            if (update == null)
                return;

            var message = update.Message;

            bool IsCommand = false;

            if (update.Type == UpdateType.CallbackQuery)
            {
                //var onCallback = new OnCallbackQuery();
                //await onCallback.CallbackQueryHandle(update.CallbackQuery, _telegramBotClient);
                return;
            }

            if (message == null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return;
            }

            foreach (TelegramCommand command in commandService.Get())
            {
                if (command.IsExecutionNeeded(message, client))
                {
                    IsCommand = true;
                    await command.Execute(message, client, context);
                    break;
                }
            }
            //if (!IsCommand)
            //{
            //    foreach (NoTelegramCommand command in noCommandService.Get())
            //    {
            //        if (command.IsExecutionNeeded(message, _telegramBotClient))
            //        {
            //            await command.Execute(message, _telegramBotClient);
            //            break;
            //        }
            //    }
            //}
        }
    }
}
