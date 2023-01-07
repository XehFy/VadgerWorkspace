using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using VadgerWorkspace.Data.Entities;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure;
using VadgerWorkspace.Infrastructure.Keyboards;
using static System.Net.Mime.MediaTypeNames;

namespace VadgerWorkspace.Domain.Commands.Client.Waiting
{
    internal class ResendOtherTypes : NoTelegramCommand
    {
        public override async Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            
            ClientRepository clientRepository = new ClientRepository(context);
            var client = clientRepository.GetClientByIdSync(message.Chat.Id);
            client.IsActive = true;
            clientRepository.Update(client);
            clientRepository.SaveSync();

            EmployeeRepository employeeRepository = new EmployeeRepository(context);
            var employee = employeeRepository.GetEmployeeByIdSync((long)client.EmployeeId);

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

            var adminsGlob = employeeRepository.GetAllGlobalAdmins();
            var text = $"От клиента {client.Name}\n сообщение не текстового типа";
            //foreach (var admin in adminsGlob)
            //{
            //    await clientBot.SendTextMessageAsync(admin.Id, text);
            //    await clientBot.CopyMessageAsync(admin.Id, client.Id, message.MessageId);
            //}
            if (employee != null)
            {
                await clientBot.SendTextMessageAsync(employee.Id, text);
                await clientBot.CopyMessageAsync(employee.Id, client.Id, message.MessageId);

            }
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
