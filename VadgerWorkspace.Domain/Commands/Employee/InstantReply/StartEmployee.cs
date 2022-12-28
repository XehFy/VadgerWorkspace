using Microsoft.EntityFrameworkCore;
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
using Telegram.Bot.Types.ReplyMarkups;

namespace VadgerWorkspace.Domain.Commands.Employee.InstantReply
{
    internal class StartEmployee : TelegramCommand
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
                client = new Data.Entities.Employee
                {
                    Name = message.Chat.FirstName,
                    Id = message.Chat.Id,
                    Stage = Data.Stages.SelectService
                };
                employeeRepository.Create(client);
                
                await employeeRepository.SaveAsync();
            }
            else
            {
                
                //var mes = await employeeBot.SendTextMessageAsync(
                //message.Chat.Id,
                //"Вы уже зарегистрированы как работник",
                //replyMarkup: KeyboardEmployee.Menu);
                client.Stage = Data.Stages.SelectService;
                employeeRepository.Update(client);
                await employeeRepository.SaveAsync();
            }
            if (client.IsAdmin == false)
            {
                var admins = employeeRepository.GetAllAdmins();
                var empKeyboard = KeyboardEmployee.ChooseMaster(admins);
                await employeeBot.SendTextMessageAsync(client.Id, "Выберете своего управляющего", replyMarkup: new InlineKeyboardMarkup(empKeyboard));
            }
            else await employeeBot.SendTextMessageAsync(client.Id, "вы являетесь администаротом и можете назначать клиентов себе", replyMarkup: KeyboardEmployee.Menu);
            employeeRepository.Dispose();
        }
    }
}
