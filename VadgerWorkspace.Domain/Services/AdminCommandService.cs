using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Commands.Admin;
using VadgerWorkspace.Domain.Commands.Admin.InstantReply;

namespace VadgerWorkspace.Domain.Services
{
    public class AdminCommandService : ICommandService
    {
        private readonly List<INorTelegramCommand> _commands;

        public AdminCommandService()
        {
            _commands = new List<INorTelegramCommand>
            {
                new StartAdmin(),
                new ManagementAdmin(),
                new ShowMessages(),
            };
        }

        public List<INorTelegramCommand> Get() => _commands;
    }
}
