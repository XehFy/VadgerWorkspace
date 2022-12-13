using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace VadgerWorkspace.Infrastructure.Keyboards
{
    public class SelectClientInline
    {
        public static InlineKeyboardMarkup SelectClient = new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData ("Посмотреть диалог клиента", "123")
        });
    }
}
