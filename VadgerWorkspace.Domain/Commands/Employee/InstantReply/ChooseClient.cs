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
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure;
using VadgerWorkspace.Infrastructure.Keyboards;

namespace VadgerWorkspace.Domain.Commands.Employee.InstantReply
{
    public class ChooseClient : TelegramCommand
    {
        public override string Name => "Чат с клиентом";

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            var employeeId = message.Chat.Id;

            ClientRepository clientRepository = new ClientRepository(context);
            var clients = await clientRepository.GetAllClientsForEmployee(employeeId);

            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var employee = employeeRepository.GetEmployeeByIdSync(employeeId);

            var emplkeyboard = KeyboardEmployee.CreateChooseClientKeyboard(clients, employee);

            await employeeBot.SendTextMessageAsync(employeeId, "Выбери клиента", replyMarkup: new InlineKeyboardMarkup(emplkeyboard));
        }
    }
}
