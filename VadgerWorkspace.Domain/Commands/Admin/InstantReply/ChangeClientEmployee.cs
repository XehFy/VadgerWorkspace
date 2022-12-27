﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure.Keyboards;
using Microsoft.EntityFrameworkCore;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Data.Entities;
using VadgerWorkspace.Infrastructure;
using Telegram.Bot.Types.ReplyMarkups;


namespace VadgerWorkspace.Domain.Commands.Admin.InstantReply
{
    internal class ChangeClientEmployee : TelegramCommand
    {
        public override string Name => "Изменить назначенного сотрудника";

        public async override Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(context);

            var currentAdmin = employeeRepository.GetAllGlobalAdmins().FirstOrDefault(e => e.Id == message.Chat.Id);

            if (currentAdmin == null)
            {
                await adminBot.SendTextMessageAsync(message.Chat.Id, "У вас нет прав", replyMarkup: KeyboardAdmin.Empty);
                return;
            }

            ClientRepository clientRepository = new ClientRepository(context);
            var clients = clientRepository.FindAll();

            var clikeyboard = KeyboardAdmin.CreateChangeEmplKeyboard(clients);

            await adminBot.SendTextMessageAsync(message.Chat.Id, "Выберите клиента для изменения", replyMarkup: new InlineKeyboardMarkup(clikeyboard));
        }

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
    }
}