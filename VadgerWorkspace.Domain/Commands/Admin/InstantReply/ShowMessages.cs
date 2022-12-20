using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VadgerWorkspace.Data.Entities;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure;
using VadgerWorkspace.Infrastructure.Keyboards;

namespace VadgerWorkspace.Domain.Commands.Admin.InstantReply
{
    internal class ShowMessages : TelegramCommand
    {
        public override string Name => "Посмотреть переписку";

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var admin = await employeeRepository.GetEmployeeByIdAsync(message.Chat.Id);

            if (admin == null) return;

            if (admin.IsAdmin == false)
            {
                await adminBot.SendTextMessageAsync(message.Chat.Id, "Дождитесь подтверждения ваших прав", replyMarkup: KeyboardAdmin.Empty);
                return;
            }           

            if (admin.Town == null)
            {
                admin.Town = "Город не назначен";

            }

            IEnumerable<Data.Entities.Employee> employees;
            if (admin.IsLocalAdmin)
            {
                if ( admin.Town == null)
                {
                    await adminBot.SendTextMessageAsync(message.Chat.Id, "У вас нет назначеных городов", replyMarkup: KeyboardAdmin.LocalMenu);
                    return;
                }
                employees = employeeRepository.GetAllEmpsWithTown(admin.Town);
                var empKeyboard = KeyboardAdmin.CreateChooseEmployeeMessageKeyboard(employees);
                await adminBot.SendTextMessageAsync(message.Chat.Id, "выберете сотрудника", replyMarkup: new InlineKeyboardMarkup(empKeyboard));
            } else
            {
                employees = employeeRepository.FindAll();
                var empKeyboard = KeyboardAdmin.CreateChooseEmployeeMessageKeyboard(employees);
                await adminBot.SendTextMessageAsync(message.Chat.Id, "выберете сотрудника", replyMarkup: new InlineKeyboardMarkup(empKeyboard));
            }
            
            
 
        }
    }
}
