using DiscordBotDotNet.Application;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotDotNet.Commands
{
    public class QuoteCommands : BaseCommandModule
    {
        private readonly QuoteApp _Quote;

        public QuoteCommands(QuoteApp quote)
        {
            _Quote = quote;
        }

        [Command("quote")]
        public async Task AddQuote(CommandContext ctx, string flag, DiscordMember member, [RemainingText] string quote)
        {
            if (flag != "-a")
                return;

            await _Quote.AddQuote(member.DisplayName, quote);
            await ctx.RespondAsync("Quote successfully added.");
        }

        [Command("quote")]
        public async Task GetRandomQuote(CommandContext ctx)
        {
            var quote = _Quote.GetRandomQuote();

            await ctx.RespondAsync($"{quote.Content.Trim()}  ―{quote.Author}");
        }
    }
}
