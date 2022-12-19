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

            var admin = await employeeRepository.GetEmployeeByIdAsync(employeeId);

            if (admin == null)
            {
                admin = new Data.Entities.Employee
                {
                    Name = message.Chat.FirstName,
                    Id = message.Chat.Id,
                    Stage = Data.Stages.SelectService,
                    IsAdmin = false,
                    IsLocalAdmin = false,
                };
                employeeRepository.Create(admin);
                await employeeRepository.SaveAsync();
            }

            if (admin.IsAdmin == false)
            {
                var gAdmins = employeeRepository.GetAllGlobalAdmins();
                foreach (var gAdmin in gAdmins)
                {
                    await adminBot.SendTextMessageAsync(gAdmin.Id, $"{message.Chat.FirstName} прислал запрос на подтверждение прав администратора, теперь вы можете назначить его роль и города в меню управления работниками");
                }
                await adminBot.SendTextMessageAsync(employeeId, "Дождитесь решения по вопросу предоставления вам прав администратора");
            }

            if (admin.IsAdmin == true && admin.IsLocalAdmin == false)
            {
                await adminBot.SendTextMessageAsync(employeeId, "Вы уже являетесь глобальным администратором", replyMarkup: KeyboardAdmin.Menu);
            }

            if (admin.IsAdmin == true && admin.IsLocalAdmin == true)
            {
                await adminBot.SendTextMessageAsync(employeeId, "Вы уже являетесь локальным администратором", replyMarkup: KeyboardAdmin.LocalMenu);
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
