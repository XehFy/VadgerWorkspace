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

namespace VadgerWorkspace.Domain.Commands.Client.Waiting
{
    public class ResendToEmployee : NoTelegramCommand
    {
        public override async Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            var text = $"{message.Text}";

            ClientRepository clientRepository = new ClientRepository(context);
            var client = clientRepository.GetClientByIdSync(message.Chat.Id);

            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var employee = employeeRepository.GetEmployeeByIdSync((long)client.EmployeeId);

            MessageRepository messageRepository = new MessageRepository(context);
            var saveMessage = new SavedMessage() { Text = text, ClientId = client.Id, IsFromClient = true, EmployeeId = employee.Id };
            messageRepository.Create(saveMessage);
            await messageRepository.SaveAsync();

            await employeeBot.SendTextMessageAsync(employee.Id, text);
        }

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            ClientRepository clientRepository = new ClientRepository(context);
            var client = clientRepository.GetClientByIdSync(message.Chat.Id);

            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            if (client == null) return false;
            if (client.EmployeeId == null) return false;

            var employee = employeeRepository.GetEmployeeByIdSync((long)client.EmployeeId);

            //Найти к имплоику клиента с чатинг стейдж


            //clientRepository.Dispose();
            return client.Stage == Data.Stages.Chating;
        }
    }
}
