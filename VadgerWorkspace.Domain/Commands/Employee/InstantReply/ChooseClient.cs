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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var employee = employeeRepository.GetEmployeeByIdSync(employeeId);

            

            // В перспективе тут надо обновить сообщения у админов шобы тупа убрать инлайн клаву
            var clientsRepl = clientRepository.FindAll().Where(client => client.EmployeeId == employeeId && (client.IsActive == true || client.IsActive == null) && client.IsReplayed == true).OrderBy(c => c.LastOrder);
            var clientsNotRepl = clientRepository.FindAll().Where(client => client.EmployeeId == employeeId && (client.IsActive == true || client.IsActive == null) && client.IsReplayed == false).OrderBy(c => c.LastOrder);

            var keyBoardNotRepl = KeyboardEmployee.CreateChooseClientKeyboard(clientsNotRepl, employee);
            var keyBoardRepl = KeyboardEmployee.CreateChooseClientKeyboard(clientsRepl, employee);

            await employeeBot.SendTextMessageAsync(employeeId, "Вы не ответили этим клиентам:", replyMarkup: new InlineKeyboardMarkup(keyBoardNotRepl));

            await employeeBot.SendTextMessageAsync(employeeId, "Этим ответили:", replyMarkup: new InlineKeyboardMarkup(keyBoardRepl));

        }
    }
}
