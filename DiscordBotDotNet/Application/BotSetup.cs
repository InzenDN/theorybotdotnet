using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotDotNet.Application
{
    public class BotSetup
    {
        private readonly DiscordClient _Discord;

        public BotSetup(DiscordClient discord)
        {
            _Discord = discord;
        }
        public Task RunAsync()
        {
            _Discord.Ready += OnReady;
            _Discord.GuildMemberAdded += Greet;
            // _Discord.MessageReactionAdded += AddReaction;

            return Task.CompletedTask;
        }

        private async Task AddReaction(MessageReactionAddEventArgs e)
        {
            await Task.Delay(new Random().Next(0, 20000));
            await e.Message.CreateReactionAsync(e.Guild.Emojis.Where(x => x.Value.Name == "Thinkies").FirstOrDefault().Value);
        }

        private static async Task Greet(GuildMemberAddEventArgs e)
        {
            await e.Guild.GetDefaultChannel().SendMessageAsync($"Welcome to **{e.Guild.Name}**, {e.Member.DisplayName}\nType \"!help\" for my list of commands.\nIf nothing in this server makes any sense to you, that is perfectly normal.");
        }

        private static Task OnReady(ReadyEventArgs e)
        {
            Console.WriteLine("Client is ready");
            return Task.CompletedTask;
        }
    }
}
