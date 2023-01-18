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
    internal class UpdateKeyboards : TelegramCommand
    {
        public override string Name => "Обновить";

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

            var requester = await employeeRepository.GetEmployeeByIdAsync(employeeId);

            if (requester == null)
            {
                return;
            }

            var admins = employeeRepository.GetAllAdmins();
            string text = "Обновлено. Теперь можно включать и отключать клиентов, чтобы показывало меньше кнопок";


            foreach (var admin in admins) 
            {
                if (admin.IsAdmin == true && admin.IsLocalAdmin == false)
                {
                    await adminBot.SendTextMessageAsync(admin.Id, text, replyMarkup: KeyboardAdmin.Menu);
                }

                if (admin.IsAdmin == true && admin.IsLocalAdmin == true)
                {
                    await adminBot.SendTextMessageAsync(admin.Id, text, replyMarkup: KeyboardAdmin.LocalMenu);
                }
            }
            
            //admin.Stage = Data.Stages.SelectService;
            //employeeRepository.Update(admin);
            //await employeeRepository.SaveAsync();

            employeeRepository.Dispose();


            //var mes = await adminBot.SendTextMessageAsync(
            //    message.Chat.Id,
            //    "Вы зарегистрировались как Admin",
            //    replyMarkup: KeyboardAdmin.Menu);
        }
    }
}
