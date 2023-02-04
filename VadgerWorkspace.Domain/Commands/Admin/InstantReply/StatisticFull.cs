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
using VadgerWorkspace.Data.Entities;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure;
using VadgerWorkspace.Infrastructure.Keyboards;

namespace VadgerWorkspace.Domain.Commands.Admin.InstantReply
{
    internal class StatisticFull : TelegramCommand
    {
        public override string Name => "Полная сатистика";

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
            var ServiceCount = clientRepository.FindAll().Where(c => c.Service != null).Count();
            var clientsTown = clientRepository.FindAll().Where(c => c.Town != null);
            var TownCount = clientsTown.Count();


            var ActiveClients = clientRepository.FindAll().Where(c => c.IsActive == true).Count();


            double ServiceP = (double)ServiceCount / (double)StartCount;
            double TownP = (double)TownCount / (double)StartCount;
            double ActiveP = (double)ActiveClients / (double)StartCount;

            string statistic = $"Статистика полная\nНажатий старт {StartCount}" +
                $"\nВыбрали услугу {ServiceCount} ({ServiceP.ToString("P", CultureInfo.InvariantCulture)})" +
                $"\nВыбрали город {TownCount} ({TownP.ToString("P", CultureInfo.InvariantCulture)})" +
                $"\nАктивны на данный момент {ActiveClients} ({ActiveP.ToString("P", CultureInfo.InvariantCulture)})" +
                $"";


            #region ClientsInfo
            int replayedClients = 0;
            var messRepository = new MessageRepository(context);
            foreach (var client in clientsTown)
            {

                var mess = messRepository.FindByCondition(m => m.ClientId == client.Id && m.IsFromClient == true).FirstOrDefault();
                if (mess != null)
                {
                    replayedClients++;
                }
            }
            double replayedClientsP = (double)replayedClients / (double)StartCount;
            statistic += $"\nНаписали хотя бы одно сообщение {replayedClients} ({replayedClientsP.ToString("P", CultureInfo.InvariantCulture)})";

            #endregion

            #region EmplInfo


            #endregion

            #region Service
            statistic += "\n\nУСЛУГИ";

            foreach (var service in KeyboardClient.Services)
            {
                var countService = clientsTown.Where(c => c.Service == service).Count();
                double countServP = (double)countService / (double)TownCount;
                statistic += $"\n{service} {countService} ({countServP.ToString("P", CultureInfo.InvariantCulture)})";
            }
            List<Data.Entities.Client> clientOtherService = new List<Data.Entities.Client>();

            foreach (var client in clientsTown)
            {
                if (!KeyboardClient.Services.Contains(client.Service))
                {
                    clientOtherService.Add(client);
                }
            }

            var countOtherService = clientOtherService.Count();
            double countOtherServP = (double)countOtherService / (double)TownCount;
            statistic += $"\nДругие {countOtherService} ({countOtherServP.ToString("P", CultureInfo.InvariantCulture)})";

            #endregion


            #region Towns
            statistic += "\n\nГОРОДА";
            foreach (var town in KeyboardClient.Towns)
            {
                var countTown = clientsTown.Where(c => c.Town == town).Count();
                double countTownP = (double)countTown / (double)TownCount;
                statistic += $"\n{town} {countTown} ({countTownP.ToString("P", CultureInfo.InvariantCulture)})";
            }
            List<Data.Entities.Client> clientOtherTown = new List<Data.Entities.Client>();

            foreach (var client in clientsTown)
            {
                if (!KeyboardClient.Towns.Contains(client.Town))
                {
                    clientOtherTown.Add(client);
                }
            }

            var countOtherTown = clientOtherTown.Count();
            double countOtherTownP = (double)countOtherTown / (double)TownCount;
            statistic += $"\nГорода не с кнопок {countOtherTown} ({countOtherTownP.ToString("P", CultureInfo.InvariantCulture)})";

            #endregion


            #region ServiceByTown

            statistic += "\n\nУСЛУГИ ПО ГОРОДАМ";
            foreach (var town in KeyboardClient.Towns)
            {
                var countTown = clientsTown.Where(c => c.Town == town).Count();
                statistic += $"\n\n{town} ({countTown})";

                foreach (var service in KeyboardClient.Services)
                {
                    var countService = clientsTown.Where(c => c.Service == service && c.Town == town).Count();
                    double countServP = (double)countService / (double)countTown;
                    if (countService != 0)
                    {
                        statistic += $"\n{service} {countService} ({countServP.ToString("P", CultureInfo.InvariantCulture)})";
                    }
                }
            }

            #endregion

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
