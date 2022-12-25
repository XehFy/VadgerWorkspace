using System;
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

namespace VadgerWorkspace.Domain.Commands.Client.Waiting
{
    public class SelectTownClient : NoTelegramCommand
    {
        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            ClientRepository clientRepository = new ClientRepository(context);
            var clientId = message.Chat.Id;


            var client = clientRepository.GetClientByIdSync(clientId);

            if (client == null) return false;


            return client.Stage == Data.Stages.SelectTown;
        }

        public override async Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            ClientRepository clientRepository = new ClientRepository(context);
            var clientId = message.Chat.Id;

            var client = clientRepository.GetClientByIdSync(clientId);
            client.Town = message.Text;
            client.Stage = Data.Stages.Waiting;
            clientRepository.Update(client);
            await clientRepository.SaveAsync();

            //clientRepository.Dispose();

            string text = $"Ваша заявка отправлена.";
            var mes = await clientBot.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: KeyboardClient.Menu);


            string requestDesc = $"Поступила заявка от: {client.Name}! \n Услуга: {client.Service} \n Город: {client.Town}\n Выберите работника:";
            //Здесь отправка инлайна с работниками и этим клиентом админу
            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var admins = employeeRepository.GetAllLocalsWithTown(client.Town);
            if (admins.Any())
            {
                var employees = employeeRepository.GetAllEmpsWithTown(client.Town);
                var empKeyboard = KeyboardAdmin.CreateChooseEmployeeKeyboard(employees, client);
                foreach (var admin in admins)
                {
                    await adminBot.SendTextMessageAsync(admin.Id, requestDesc, replyMarkup: new InlineKeyboardMarkup(empKeyboard));
                }
            }
            else
            {
                var employees = employeeRepository.FindAll();
                var adminsGlob = employeeRepository.GetAllGlobalAdmins();
                var empKeyboard = KeyboardAdmin.CreateChooseEmployeeKeyboard(employees, client);
                foreach (var admin in admins)
                {
                    await adminBot.SendTextMessageAsync(admin.Id, requestDesc, replyMarkup: new InlineKeyboardMarkup(empKeyboard));
                }
            }
        }
    }
}
