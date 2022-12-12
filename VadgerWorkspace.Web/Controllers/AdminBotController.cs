﻿using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace VadgerWorkspace.Web.Controllers
{
    [ApiController]
    [Route("/AdminBot")]
    public class AdminBotController : Controller
    {
        readonly IAdminBot _client;

        public AdminBotController(IAdminBot client)
        {
            _client = client;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                await _client.SendTextMessageAsync(update.Message.Chat.Id, "TESTING_ADMIN");
            }
            return Ok();
        }
    }
}