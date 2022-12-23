using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Microsoft.EntityFrameworkCore;
using VadgerWorkspace.Infrastructure;
using VadgerWorkspace.Data;
using VadgerWorkspace.Data.Repositories;

namespace VadgerWorkspace.Domain.Commands.Employee
{
    public class OnCallbackQueryEmployee
    {
        public async Task CallbackQueryEmployeeHandle(CallbackQuery query, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, VadgerContext context)
        {
            var cbargs = query.Data.Split(' ');
            switch (cbargs[0])
            {
                case "/StartChat":
                    var employeeId = Convert.ToInt64(cbargs[1]);
                    var clientId = Convert.ToInt64(cbargs[2]);

                    //await clientBot.SendTextMessageAsync(clientId, "Был открыт чат с нашим работником, все последующие сообщения будут поступать от него и записываться в нашу систему");
                    await employeeBot.SendTextMessageAsync(employeeId, "Вы открыли чат с клиентом");//, дальнейшие сообщения будут направлены клиенту и записываться для контроля качества.

                    ClientRepository clientRepository = new ClientRepository(context);
                    var client = clientRepository.GetClientByIdSync(clientId);
                    client.EmployeeId = employeeId;
                    client.Stage = Stages.Chating;
                    clientRepository.Update(client);

                    EmployeeRepository employeeRepository = new EmployeeRepository(context);
                    var employee = await employeeRepository.GetEmployeeByIdAsync(employeeId);
                    employee.Stage = Stages.Chating;
                    employee.ClientId = clientId;
                    employeeRepository.Update(employee);

                    await clientRepository.SaveAsync();
                    clientRepository.Dispose();
                    break;
            }
        }
    }
}
