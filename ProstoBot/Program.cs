using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProstoBot;
using Telegram.Bot;
using ProstoBot.Controllers;

namespace ProstoBot
{
    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("6302779481:AAHtTRzBOF4VpurScSce9h0UAnzB6kmwUMI"));
            
            // Подключаем контроллеры сообщений и кнопок
            services.AddTransient<DefaultMessageController>();            
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }
    }
}