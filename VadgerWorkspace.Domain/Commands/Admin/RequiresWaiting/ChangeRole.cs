using System;
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

namespace VadgerWorkspace.Domain.Commands.Admin.RequiresWaiting
{
    public class ChangeRole : TelegramCommand
    {
        public override string Name => "Изменить роль";

        public async override Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var descider = await employeeRepository.GetEmployeeByIdAsync(message.Chat.Id);
            if (descider.Stage == Data.Stages.Management && descider.IsAdmin == true && descider.IsLocalAdmin == false)
            {
                await adminBot.SendTextMessageAsync(message.Chat.Id, "Введите роль", replyMarkup: KeyboardAdmin.Roles);
            }
            else
            {
                await adminBot.SendTextMessageAsync(message.Chat.Id, "иди нахой");
            }
        }

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
    }
}
