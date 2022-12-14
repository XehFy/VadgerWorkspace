using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VadgerWorkspace.Data;
using VadgerWorkspace.Data.Entities;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Services;
using VadgerWorkspace.Infrastructure;

namespace VadgerWorkspace.Web.Controllers
{
    [ApiController]
    [Route("/AdminBot")]
    public class AdminBotController : Controller
    {
        readonly VadgerContext _context;

        readonly IAdminBot _adminBot;
        readonly IClientBot _clientBot;
        readonly IEmployeeBot _employeeBot;
        private readonly ICommandService _commandService;
        private readonly ICommandService _noCommandService;

        public AdminBotController(IAdminBot adminBot, IClientBot clientBot, IEmployeeBot employeeBot, IEnumerable<ICommandService> commandServices, VadgerContext context)
        {
            _commandService = commandServices.First(o => o.GetType() == typeof(AdminCommandService));
            _clientBot = clientBot;
            _adminBot = adminBot;
            _employeeBot = employeeBot;

            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
        {
            var commandService = (AdminCommandService)_commandService;

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
                if (command.IsExecutionNeeded(message, _clientBot, _employeeBot, _adminBot, _context))
                {
                    IsCommand = true;
                    await command.Execute(message, _clientBot, _employeeBot, _adminBot, _context);
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
