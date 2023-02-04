using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    internal class StatisticsShort : TelegramCommand
    {
        public override string Name => "Статистика";

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
            var StartCount = clientRepository.FindAll().Count();
            var ServiceCount = clientRepository.FindAll().Where(c=>c.Service != null).Count();
            var TownCount = clientRepository.FindAll().Where(c => c.Town != null).Count();

            var ActiveClients = clientRepository.FindAll().Where(c => c.IsActive == true).Count();


            double ServiceP = (double)ServiceCount / (double)StartCount;
            double TownP = (double)TownCount / (double)StartCount;
            double ActiveP = (double)ActiveClients / (double)StartCount;

            string statistic = $"Статистика полная\nНажатий старт {StartCount}" +
                $"\nВыбрали услугу {ServiceCount} ({ServiceP.ToString("P", CultureInfo.InvariantCulture)})" +
                $"\nВыбрали город {TownCount} ({TownP.ToString("P", CultureInfo.InvariantCulture)})" +
                $"\nАктивны на данный момент {ActiveClients} ({ActiveP.ToString("P", CultureInfo.InvariantCulture)})" +
                $"";
            await adminBot.SendTextMessageAsync(message.Chat.Id, statistic);

            //var clikeyboardNR = KeyboardAdmin.CreateGetLinkKeyboard(clientsNotRegistred);
            //if (clientsNotRegistred.Any()) 
            //{
            //    await adminBot.SendTextMessageAsync(message.Chat.Id, "Эти клиенты нажали старт в боте, но не заказали услугу", replyMarkup: new InlineKeyboardMarkup(clikeyboardNR));
            //}

        }

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
    }
}
