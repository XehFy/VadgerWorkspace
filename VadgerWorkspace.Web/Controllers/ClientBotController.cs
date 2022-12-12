using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                await _client.SendTextMessageAsync(update.Message.Chat.Id, "TESTING_CLIENT");
            }
            return Ok();
        }
    }
}
