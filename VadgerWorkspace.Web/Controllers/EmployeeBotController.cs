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
    [Route("/EmployeeBot")]
    public class EmployeeBotController : Controller
    {
        readonly VadgerContext _context;

        readonly IAdminBot _adminBot;
        readonly IClientBot _clientBot;
        readonly IEmployeeBot _employeeBot;
        private readonly ICommandService _commandService;
        private readonly ICommandService _noCommandService;

        public EmployeeBotController(IAdminBot adminBot, IClientBot clientBot, IEmployeeBot employeeBot, IEnumerable<ICommandService> commandServices, VadgerContext context)
        {
            _clientBot = clientBot;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                await _employeeBot.SendTextMessageAsync(update.Message.Chat.Id, "TESTING_EMPLOYEE");
            }
            return Ok();
        }
    }
}
