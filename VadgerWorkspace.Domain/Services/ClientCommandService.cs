using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Commands.Client;

namespace VadgerWorkspace.Domain.Services
{
    public class ClientCommandService
    {
        private readonly List<INorTelegramCommand> _commands;

        public ClientCommandService()
        {
            _commands = new List<INorTelegramCommand>
            {
                new StartClient()
            };
        }

        public List<INorTelegramCommand> Get() => _commands;
    }
}
