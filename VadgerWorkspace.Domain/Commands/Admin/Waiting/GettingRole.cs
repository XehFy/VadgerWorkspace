using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
    internal class GettingRole : NoTelegramCommand
    {
        public override async Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            var role = message.Text;
            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var descider = await employeeRepository.GetEmployeeByIdAsync(message.Chat.Id);

            var employee = await employeeRepository.GetEmployeeByIdAsync((long)descider.ManagementId);
            bool flag = false;

            if (role.Contains("Глобальный админ"))
            {
                employee.IsLocalAdmin = false;
                employee.IsAdmin = true;
                flag = true;
                await adminBot.SendTextMessageAsync(employee.Id, "Вам назначена роль Глобальный админ", replyMarkup: KeyboardAdmin.Menu);
            }
            if (role.Contains("Локальный админ"))
            {
                employee.IsLocalAdmin = true;
                employee.IsAdmin = true;
                flag = true;
                await adminBot.SendTextMessageAsync(employee.Id, "Вам назначена роль Локальный админ", replyMarkup: KeyboardAdmin.LocalMenu);
            }
            if (role.Contains("Сотрудник"))
            {
                employee.IsLocalAdmin = false;
                employee.IsAdmin = false;
                flag = true;
                await employeeBot.SendTextMessageAsync(employee.Id, "Вам назначена роль Сотрудник", replyMarkup: KeyboardEmployee.Menu);
            }
            if (flag == false)
            {
                return;
            }
            descider.Stage = Data.Stages.SelectService;

            employeeRepository.Update(employee);
            employeeRepository.Update(descider);
            await employeeRepository.SaveAsync();
            employeeRepository.Dispose();
            await adminBot.SendTextMessageAsync(message.Chat.Id, "Роль назначена", replyMarkup: KeyboardAdmin.Menu);
            
        }

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var empl = employeeRepository.GetEmployeeByIdSync(message.Chat.Id);
            //employeeRepository.Dispose();
            return empl.Stage == Data.Stages.Management;
        }
    }
}
