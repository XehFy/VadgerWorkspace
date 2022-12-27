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
    public class ChangeTown : TelegramCommand
    {
        public override string Name => "Изменить города";

        public async override Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var descider = await employeeRepository.GetEmployeeByIdAsync(message.Chat.Id);
            if (descider.Stage == Data.Stages.Management && descider.IsAdmin == true && descider.IsLocalAdmin == false)
            {
                descider.Stage = Data.Stages.SelectTown;
                employeeRepository.Update(descider);
                await employeeRepository.SaveAsync();

                await adminBot.SendTextMessageAsync(message.Chat.Id, "Если вы хотите изменить города для админа то также не забудьте изменить города для всех его сотрудников\nВведите города с большой буквы через пробел. \nС клавиатуры будет выбран только один город", replyMarkup: KeyboardAdmin.SelectTown);
            }
            else
            {
                await adminBot.SendTextMessageAsync(message.Chat.Id, "У вас нет доступа");
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
