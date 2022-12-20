using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VadgerWorkspace.Data.Entities;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure;

namespace VadgerWorkspace.Domain.Commands.Employee.Waiting
{
    internal class ResendToClient : NoTelegramCommand
    {
        public override async Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            var text = $"{message.Text}";
            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var employee = employeeRepository.GetEmployeeByIdSync(message.Chat.Id);

            ClientRepository clientRepository = new ClientRepository(context);
            var client = clientRepository.GetClientByIdSync(employee.ClientId);

            MessageRepository messageRepository = new MessageRepository(context);
            var saveMessage = new SavedMessage() { Text = text, ClientId = client.Id, IsFromClient = false, EmployeeId = employee.Id, Time = message.Date };
            messageRepository.Create(saveMessage);
            await messageRepository.SaveAsync();

            await clientBot.SendTextMessageAsync(client.Id, "От сотрудника\n" + text);
        }

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;
            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var employee = employeeRepository.GetEmployeeByIdSync(message.Chat.Id);

            //Найти к имплоику клиента с чатинг стейдж
            ClientRepository clientRepository = new ClientRepository(context);
            if (employee.ClientId == 0) return false;
            var client = clientRepository.GetClientByIdSync(employee.ClientId);

            //clientRepository.Dispose();
            return client.Stage == Data.Stages.Chating && employee.Stage == Data.Stages.Chating;
        }
    }
}
