﻿using Microsoft.EntityFrameworkCore;
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

namespace VadgerWorkspace.Domain.Commands.Admin.InstantReply
{
    public class GetClientLink : TelegramCommand
    {
        public override string Name => "Получить ссылку на клиента";

        public async override Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            ClientRepository clientRepository = new ClientRepository(context);
            var clients = clientRepository.FindAll();

            var clikeyboard = KeyboardAdmin.CreateGetLinkKeyboard(clients);

            await adminBot.SendTextMessageAsync(message.Chat.Id, "Выберите клиента на которого хотите получить ссылку", replyMarkup: new InlineKeyboardMarkup(clikeyboard));
        }

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
    }
}