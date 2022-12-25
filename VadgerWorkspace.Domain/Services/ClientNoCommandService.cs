using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Commands.Client.Waiting;

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
                new ResendToEmployee(),
                new ResendOtherTypes(),
            };
        }

        public List<INorTelegramCommand> Get() => _commands;
    }
}