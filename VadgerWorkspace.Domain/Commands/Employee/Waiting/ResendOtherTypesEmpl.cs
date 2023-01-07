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
using static System.Net.Mime.MediaTypeNames;

namespace VadgerWorkspace.Domain.Commands.Employee.Waiting
{
    internal class ResendOtherTypesEmpl : NoTelegramCommand
    {
        public override async Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {

            

            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var employee = employeeRepository.GetEmployeeByIdSync(message.Chat.Id);

            ClientRepository clientRepository = new ClientRepository(context);
            var client = clientRepository.GetClientByIdSync(employee.ClientId);

            //if (employee != null) 
            //{
            //    switch (message.Type)
            //    {
            //        case MessageType.Voice:
            //            //await employeeBot.SendVoiceAsync(employee.Id, message.Voice.FileId);
            //            var filePath = Path.Combine(Path.GetTempPath(), message.Voice.FileId + ".ogg");

            //            // InputOnlineFile f = new InputOnlineFile(file);
            //            //await employeeBot.SendVoiceAsync(employee.Id, file);
            //            using (var file = System.IO.File.Open(filePath, FileMode.OpenOrCreate))
            //            {
            //                var filetg = await clientBot.GetFileAsync(message.Voice.FileId);
            //                //InputOnlineFile f = new InputOnlineFile(file);
            //                await clientBot.DownloadFileAsync(filetg.FilePath, file);
            //                await employeeBot.SendVoiceAsync(employee.Id, file);
            //                Console.WriteLine($"Find Voice at {filePath}");
            //            }
            //            break;
            //    }

            //}

            if (client != null)
            {

                var adminsGlob = employeeRepository.GetAllGlobalAdmins();
                var text = $"От сотрудника {employee.Name}\n сообщение не текстового типа для {client.Name}";
                foreach (var admin in adminsGlob)
                {
                    await clientBot.SendTextMessageAsync(admin.Id, text);
                    await clientBot.CopyMessageAsync(admin.Id, employee.Id, message.MessageId);
                }
                if (client != null)
                {
                    await clientBot.SendTextMessageAsync(client.Id, text);
                    await clientBot.CopyMessageAsync(client.Id, employee.Id, message.MessageId);
                    client.IsReplayed = true;
                    clientRepository.Update(client);
                }
            } else await clientBot.SendTextMessageAsync(employee.Id, "у вас сейчас нет клиента");
        }

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type == MessageType.Audio ||
                message.Type == MessageType.Document ||
                message.Type == MessageType.Photo ||
                message.Type == MessageType.Video ||
                message.Type == MessageType.Voice)
                return true;
            else return false;

        }
    }
}
