using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VadgerWorkspace.Data.Repositories;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Infrastructure;
using VadgerWorkspace.Infrastructure.Keyboards;


namespace VadgerWorkspace.Domain.Commands.Client.InstantReply.FAQ
{
    internal class FAQ4 : TelegramCommand
    {
        public override string Name => "🚗Покупка авто и получение прав";

        public async override Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            ClientRepository clientRepository = new ClientRepository(context);
            var clientId = message.Chat.Id;

            var client = await clientRepository.GetClientByIdAsync(clientId);


            //if (client.Stage == Data.Stages.Chating)
            //{
            //    await clientBot.SendTextMessageAsync(clientId, "Пожалуйста, дождитесь ответа от нашего работника", replyMarkup: KeyboardClient.Empty);
            //    return;
            //}
            ////ПОТОМ УБРАТЬ
            //client.EmployeeId = 0;
            ////УБРАТЬ НАХОЙ
            ///
            //if (client.IsChechedFAQ == null)
            //{
            //    client.IsChechedFAQ = 0;
            //}
            //var bytes = BitConverter.GetBytes((int)client.IsChechedFAQ);
            //bytes[3] = 1;
            //int resultBytes = BitConverter.ToInt32(bytes);
            //client.IsChechedFAQ = resultBytes;
            clientRepository.Update(client);
            await clientRepository.SaveAsync();

            clientRepository.Dispose();

            string text = $"faq4";
            var mes = await clientBot.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: KeyboardClient.Menu);
        }

        public override bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
    }
}
