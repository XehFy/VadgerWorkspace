using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Commands.Admin;
using VadgerWorkspace.Domain.Commands.Admin.InstantReply;
using VadgerWorkspace.Domain.Commands.Admin.RequiresWaiting;

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
                new EmployeeManagement(),
                new BackToMenu(),
                new ChangeRole(),
                new ChangeTown(),
                new ShowMessages(),
                new GetClientLink(),
                new ChangeClientEmployee(),
                new ActivateClient(),
                new DeactiveClient(),
                new UpdateKeyboards(),
                new InitDates(),
                new InitReplayes(),
                new DeactivateMass(),
                new StatisticsShort(),
                new StatisticFull(),
            };
        }

        public List<INorTelegramCommand> Get() => _commands;
    }
}
