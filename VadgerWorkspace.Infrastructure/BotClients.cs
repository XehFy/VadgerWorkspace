using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace VadgerWorkspace.Infrastructure
{
    public interface IClientBot : ITelegramBotClient
    {
    }

    public class ClientBot : TelegramBotClient, IClientBot
    {
        public ClientBot(String token, HttpClient? httpClient = null) : base(token, httpClient)
        {
        }
    }

    public interface IAdminBot : ITelegramBotClient
    {
    }

    public class AdminBot : TelegramBotClient, IAdminBot
    {
        public AdminBot(String token, HttpClient? httpClient = null) : base(token, httpClient)
        {
        }
    }

    public interface IEmployeeBot : ITelegramBotClient
    {
    }

    public class EmployeeBot : TelegramBotClient, IEmployeeBot
    {
        public EmployeeBot(String token, HttpClient? httpClient = null) : base(token, httpClient)
        {
        }
    }
}
