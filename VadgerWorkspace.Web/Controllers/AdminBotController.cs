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
using VadgerWorkspace.Domain.Commands.Admin;
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
            _noCommandService = commandServices.First(o => o.GetType() == typeof(AdminNoCommandService));
            _clientBot = clientBot;
            _adminBot = adminBot;
            _employeeBot = employeeBot;

            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
        {
            var commandService = (AdminCommandService)_commandService;
            var noCommandService = (AdminNoCommandService)_noCommandService;

            if (update == null)
                return Ok();
            //if (update.Message.Text == "Сотрудник")
            //{
            //    return Ok();
            //}

            var message = update.Message;

            bool IsCommand = false;

            if (update.Type == UpdateType.CallbackQuery)
            {
                var onCallback = new OnCallbackQueryAdmin();
                await onCallback.CallbackQueryAdminHandle(update.CallbackQuery,_clientBot, _employeeBot, _adminBot, _context);
                return Ok();
            }

            if (message == null)
            {
                //cancellationToken.ThrowIfCancellationRequested();
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
            if (!IsCommand)
            {
                foreach (NoTelegramCommand command in noCommandService.Get())
                {
                    if (command.IsExecutionNeeded(message, _clientBot, _employeeBot, _adminBot, _context))
                    {
                        await command.Execute(message, _clientBot, _employeeBot, _adminBot, _context);
                        break;
                    }
                }
            }
            _context.Dispose();
            return Ok();
        }
    }
}
