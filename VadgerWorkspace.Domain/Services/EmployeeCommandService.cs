using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Commands.Employee;

namespace VadgerWorkspace.Domain.Services
{
    public class EmployeeCommandService
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
