using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Commands.Client;
using VadgerWorkspace.Domain.Commands.Client.InstantReply;
using VadgerWorkspace.Domain.Commands.Client.InstantReply.FAQ;
//using VadgerWorkspace.Domain.Commands.Client.InstantReply;
using VadgerWorkspace.Domain.Commands.Client.RequiresWaiting;

namespace VadgerWorkspace.Domain.Services
{
    public class ClientCommandService : ICommandService
    {
        private readonly List<INorTelegramCommand> _commands;

        public ClientCommandService()
        {
            _commands = new List<INorTelegramCommand>
            {
                new StartClient(),
                new MakeOrder(),
                new StartFAQ(),
                new FAQ1(),
                new FAQ2(),
                new FAQ3(),
                new FAQ4(),
            };
        }

        public List<INorTelegramCommand> Get() => _commands;
    }
}
