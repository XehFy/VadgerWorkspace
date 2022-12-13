using Telegram.Bot;
using VadgerWorkspace.Infrastructure;

namespace VadgerWorkspace.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramBotClient(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var client = new ClientBot(configuration["RenatClientToken"]);
            var webHook = $"{configuration["Url"]}/ClientBot";
            client.SetWebhookAsync(webHook).Wait();

            return serviceCollection
                .AddTransient<IClientBot>(x => client);
        }

        public static IServiceCollection AddTelegramBotAdmin(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var client = new AdminBot(configuration["RenatAdminToken"]);
            var webHook = $"{configuration["Url"]}/AdminBot";
            client.SetWebhookAsync(webHook).Wait();

            return serviceCollection
                .AddTransient<IAdminBot>(x => client);
        }

        public static IServiceCollection AddTelegramBotEmployee(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var client = new EmployeeBot(configuration["RenatEmployeeToken"]);
            var webHook = $"{configuration["Url"]}/EmployeeBot";
            client.SetWebhookAsync(webHook).Wait();

            return serviceCollection
                .AddTransient<IEmployeeBot>(x => client);
        }
    }


}
