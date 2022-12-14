using VadgerWorkspace.Domain.Abstractions;

namespace VadgerWorkspace.Domain.Services
{
    public class EmployeeNoCommandService : ICommandService
    {
        private readonly List<INorTelegramCommand> _commands;

        public EmployeeNoCommandService()
        {
            _commands = new List<INorTelegramCommand>
            {

            };
        }

        public List<INorTelegramCommand> Get() => _commands;
    }
}