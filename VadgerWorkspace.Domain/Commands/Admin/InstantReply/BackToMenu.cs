﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure.Keyboards;
using Microsoft.EntityFrameworkCore;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Data.Entities;
using VadgerWorkspace.Infrastructure;
using Telegram.Bot.Types.ReplyMarkups;

namespace VadgerWorkspace.Domain.Commands.Admin.InstantReply
{
    public class BackToMenu : TelegramCommand
    {
        public override string Name => "Вернуться в меню";

        public async override Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var employee = employeeRepository.GetEmployeeByIdSync(message.Chat.Id);

            if (employee.IsLocalAdmin)
            {
                await adminBot.SendTextMessageAsync(message.Chat.Id, "меню", replyMarkup: KeyboardAdmin.LocalMenu);

            } else await adminBot.SendTextMessageAsync(message.Chat.Id, "меню", replyMarkup: KeyboardAdmin.Menu);
            
        }

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
    }
}
