﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Domain.Commands.Admin.Waiting;

namespace VadgerWorkspace.Domain.Services
{
    public class AdminNoCommandService : ICommandService
    {
        private readonly List<INorTelegramCommand> _commands;

        public AdminNoCommandService()
        {
            _commands = new List<INorTelegramCommand>
            {
                new GettingTown(),
                new GettingRole(),
            };
        }

        public List<INorTelegramCommand> Get() => _commands;
    }
}
