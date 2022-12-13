﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VadgerWorkspace.Data;
using VadgerWorkspace.Data.Entities;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Services;

namespace VadgerWorkspace.Web.Controllers
{
    [ApiController]
    [Route("/ClientBot")]
    public class ClientBotController : Controller
    {
        readonly IClientBot _client;

        public ClientBotController(IClientBot client)
        {
            _client = client;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
        {
            DbContext context = new VadgerContext();
            ClientCommandService commandService = new ClientCommandService();
            // NoCommandService noCommandService = (NoCommandService)_noCommandService;

            if (update == null)
                return Ok();

            var message = update.Message;

            bool IsCommand = false;

            if (update.Type == UpdateType.CallbackQuery)
            {
                //var onCallback = new OnCallbackQuery();
                //await onCallback.CallbackQueryHandle(update.CallbackQuery, _telegramBotClient);
                return Ok();
            }

            if (message == null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Ok();
            }

            foreach (TelegramCommand command in commandService.Get())
            {
                if (command.IsExecutionNeeded(message, _client))
                {
                    IsCommand = true;
                    await command.Execute(message, _client, context);
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
            return Ok();
        }
    }
}
