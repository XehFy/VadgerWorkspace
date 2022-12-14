using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Commands.Employee.Waiting;

namespace VadgerWorkspace.Domain.Services
{
    public class EmployeeNoCommandService : ICommandService
    {
        private readonly List<INorTelegramCommand> _commands;

        public EmployeeNoCommandService()
        {
            _commands = new List<INorTelegramCommand>
            {
                new ResendToClient()
            };
        }

        public List<INorTelegramCommand> Get() => _commands;
    }
}