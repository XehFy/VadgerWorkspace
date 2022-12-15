using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Commands.Client;
using VadgerWorkspace.Domain.Commands.Client.InstantReply;
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
                new MakeOrder()
            };
        }

        public List<INorTelegramCommand> Get() => _commands;
    }
}
