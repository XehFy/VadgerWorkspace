﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace VadgerWorkspace.Domain.Abstractions
{
    public abstract class NoTelegramCommand : INorTelegramCommand
    {
        public abstract Task Execute(Message message, ITelegramBotClient client, DbContext context);

        public abstract bool IsExecutionNeeded(Message message, ITelegramBotClient client);
    }
}