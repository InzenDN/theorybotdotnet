using DiscordBotDotNet.Application;
using DiscordBotDotNet.Persistence;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Lavalink;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBotDotNet
{
    public static class Startup
    {
        private static IConfiguration Configuration { get; set; }
        public static IServiceProvider Services { get; private set; }
        private static DiscordClient DiscordClient { get; set; }

        public static async Task ConfigureAsync()
        {
            ConfigureConfiguration();
            ConfigureServices();
            ConfigureDiscordCommands();

            using (var scope = Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }

            await DiscordClient.ConnectAsync();
        }

        public static async Task RunAsync()
        {
            var botSetup = Services.GetRequiredService<BotSetup>();
            await botSetup.RunAsync();

            var quoteApp = Services.GetRequiredService<QuoteApp>();
            await quoteApp.RunAsync();

            await Task.Delay(-1);
        }

        private static void ConfigureServices()
        {
            DiscordClient = DiscordClientBuilder();

            Services = new ServiceCollection()
                .AddSingleton(Configuration)
                .AddSingleton(DiscordClient)
                .AddSingleton(DiscordClient.UseLavalink())
                .AddDbContext<ApplicationDbContext>()
                .AddSingleton<BotSetup>()
                .AddSingleton<TriviaGame>()
                .AddSingleton<QuoteApp>()
                .AddSingleton<HttpRequest>()
                .BuildServiceProvider();
        }

        private static void ConfigureConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        private static DiscordClient DiscordClientBuilder()
        {
            var config = new DiscordConfiguration()
            {
                Token = Configuration["BotToken"],
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            return new DiscordClient(config);
        }

        private static void ConfigureDiscordCommands()
        {
            var cmdCfg = new CommandsNextConfiguration
            {
                StringPrefixes = new List<string> { Configuration["CmdPrefix"] },
                IgnoreExtraArguments = true,
                Services = Services,
                EnableMentionPrefix = true
            };

            var discordCmds = DiscordClient.UseCommandsNext(cmdCfg);
            discordCmds.RegisterCommands(typeof(DiceCommands).Assembly);
        }
    }
}
