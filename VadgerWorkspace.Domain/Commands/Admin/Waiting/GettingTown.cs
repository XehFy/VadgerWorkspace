using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure;
using VadgerWorkspace.Infrastructure.Keyboards;

namespace VadgerWorkspace.Domain.Commands.Admin.Waiting
{
    internal class GettingTown : NoTelegramCommand
    {
        public override async Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            var towns = message.Text;
            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var descider = await employeeRepository.GetEmployeeByIdAsync(message.Chat.Id);
            descider.Stage = Data.Stages.SelectService;
            var employee = await employeeRepository.GetEmployeeByIdAsync((long)descider.ManagementId);
            employee.Town = towns;
            //employeeRepository.Update(descider);
            context.Set<Data.Entities.Employee>().UpdateRange(descider, employee);

            //employeeRepository.Update(employee);
            context.SaveChanges();
            //await employeeRepository.SaveAsync();

            await adminBot.SendTextMessageAsync(message.Chat.Id, "Города назначены", replyMarkup: KeyboardAdmin.Menu);
        }

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var empl = employeeRepository.GetEmployeeByIdSync(message.Chat.Id);
            //employeeRepository.Dispose();
            return empl.Stage == Data.Stages.SelectTown;
        }
    }
}
