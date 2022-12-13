using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Microsoft.EntityFrameworkCore;

namespace VadgerWorkspace.Domain.Commands.Employee
{
    public class OnCallbackQueryEmployee
    {
        public async Task CallbackQueryHandle(CallbackQuery query, ITelegramBotClient bot)
        {
            await bot.SendTextMessageAsync(query.Message.Chat.Id, query.Data);
        }
    }
}
