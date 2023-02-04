using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Commands.Client.Waiting;
using VadgerWorkspace.Domain.Commands.Employee.Waiting;

namespace VadgerWorkspace.Domain.Services
{
    public class ClientNoCommandService : ICommandService
    {
        private readonly List<INorTelegramCommand> _commands;

        public ClientNoCommandService()
        {
            _commands = new List<INorTelegramCommand>
            {
                new WaitingClient(),
                new SelectServiceClient(),
                new SelectTownClient(),
                new ResendOtherTypesEmpl(),
                new ResendOtherTypes(),
            };
        }

        public List<INorTelegramCommand> Get() => _commands;
    }
}