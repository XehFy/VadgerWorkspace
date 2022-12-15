using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure.Keyboards;
using Microsoft.EntityFrameworkCore;
using VadgerWorkspace.Infrastructure;
using VadgerWorkspace.Data.Repositories;

namespace VadgerWorkspace.Domain.Commands.Admin.InstantReply
{
    public class StartAdmin : TelegramCommand
    {
        public override string Name => @"/start";

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var employeeId = message.Chat.Id;

            var client = await employeeRepository.GetEmployeeByIdAsync(employeeId);

            if (client == null)
            {
                employeeRepository.Create(new Data.Entities.Employee
                {
                    Name = message.Chat.FirstName,
                    Id = message.Chat.Id,
                    Stage = Data.Stages.SelectService,
                    IsAdmin = true
                });
                await employeeRepository.SaveAsync();
            }
            else
            {
                client.Stage = Data.Stages.SelectService;
                employeeRepository.Update(client);
                await employeeRepository.SaveAsync();
            }
            employeeRepository.Dispose();


            var mes = await adminBot.SendTextMessageAsync(
                message.Chat.Id,
                "Вы зарегистрировались как Admin",
                replyMarkup: KeyboardAdmin.Menu);
        }

    }
}
