using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Commands.Admin;

namespace VadgerWorkspace.Domain.Services
{
    public class AdminCommandService
    {
        private readonly List<INorTelegramCommand> _commands;

        public AdminCommandService()
        {
            _commands = new List<INorTelegramCommand>
            {
                new StartAdmin(),
                new ManagementAdmin(),
            };
        }

        public List<INorTelegramCommand> Get() => _commands;
    }
}
