using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Commands.Employee;
using VadgerWorkspace.Domain.Commands.Employee.InstantReply;

namespace VadgerWorkspace.Domain.Services
{
    public class EmployeeCommandService : ICommandService
    {
        private readonly List<INorTelegramCommand> _commands;

        public EmployeeCommandService()
        {
            _commands = new List<INorTelegramCommand>
            {
                new StartEmployee(),
                new SelectClientEmployee(),
            };
        }

        public List<INorTelegramCommand> Get() => _commands;
    }
}
