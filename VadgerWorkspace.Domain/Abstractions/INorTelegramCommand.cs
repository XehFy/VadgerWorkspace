using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Microsoft.EntityFrameworkCore;
using VadgerWorkspace.Infrastructure;

namespace VadgerWorkspace.Domain.Abstractions
{
    public interface INorTelegramCommand
    {
        Task Execute(Message message, IClientBot clientBot, IEmployeeBot employeeBot,IAdminBot adminBot, DbContext context);
        bool IsExecutionNeeded(Message message, IClientBot clientBot, IEmployeeBot employeeBot, IAdminBot adminBot, DbContext context);
    }
}
