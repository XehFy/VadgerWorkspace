using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VadgerWorkspace.Data;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Commands.Employee;
using VadgerWorkspace.Domain.Services;

namespace VadgerWorkspace.Domain.UpdateHandlers
{
    internal class EmployeeUpdateHandler
    {
        DbContext context = new VadgerContext();
        public async Task Handle(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            EmployeeCommandService commandService = new EmployeeCommandService();
            // NoCommandService noCommandService = (NoCommandService)_noCommandService;

            if(update == null)
                return;

            var message = update.Message;

            bool IsCommand = false;

            if(update.Type == UpdateType.CallbackQuery)
            {
                var onCallback = new OnCallbackQueryEmployee();
                await onCallback.CallbackQueryHandle(update.CallbackQuery, client);
                return;
            }

            if(message == null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return;
            }

            foreach(TelegramCommand command in commandService.Get())
            {
                if(command.IsExecutionNeeded(message, client))
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
