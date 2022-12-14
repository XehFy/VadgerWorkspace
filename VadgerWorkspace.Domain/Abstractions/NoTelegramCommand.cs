using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VadgerWorkspace.Infrastructure;

namespace VadgerWorkspace.Domain.Abstractions
{
    public abstract class NoTelegramCommand : INorTelegramCommand
    {
        public abstract Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context);

        public abstract bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context);
    }
}
