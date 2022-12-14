using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VadgerWorkspace.Data;
using VadgerWorkspace.Data.Entities;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Infrastructure;

namespace VadgerWorkspace.Domain.Commands.Admin
{
    public class OnCallbackQueryAdmin
    {
        public async Task CallbackQueryAdminHandle(CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            var cbargs = query.Data.Split(' ');
            switch (cbargs[0])
            {
                case "/chooseEmp":
                    ClientRepository clientRepository = new ClientRepository(context);
                    var employeeId = Convert.ToInt64(cbargs[1]);
                    var clientId = Convert.ToInt64(cbargs[2]);

                    var client = clientRepository.GetClientByIdSync(clientId);
                    client.EmployeeId = employeeId;
                    clientRepository.Update(client);
                    var request = $"Вам назначен клиент:\n {client.Name}, {client.Town}\n{client.Service}";

                    MessageRepository messageRepository = new MessageRepository(context);
                    messageRepository.Create(new SavedMessage { ClientId = clientId, EmployeeId = employeeId, IsFromClient = false, Text = request });

                    await clientRepository.SaveAsync();

                    await employeeBot.SendTextMessageAsync(employeeId, request);
                    // В перспективе тут надо обновить сообщения у админов шобы тупа убрать инлайн клаву

                    clientRepository.Dispose();

                    break;
            }
        }
    }
}
