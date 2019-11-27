using DiscordBotDotNet.Domain;
using DiscordBotDotNet.Persistence;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotDotNet.Application
{
    public class QuoteApp
    {
        private readonly ApplicationDbContext _Context;
        private readonly DiscordClient _Discord;

        public QuoteApp(ApplicationDbContext context, DiscordClient discord)
        {
            _Context = context;
            _Discord = discord;
        }

        public async Task RunAsync()
        {
            _Discord.MessageReactionAdded += AddQuote;
            await Task.CompletedTask;
        }

        private async Task AddQuote(MessageReactionAddEventArgs e)
        {
            if (e.Emoji.Name != "🔊")
                return;

            if (e.Channel.IsNSFW || e.Channel.IsPrivate || e.Channel.Id == 393472230371360779)
                return;

            ulong msgId = e.Message.Id;
            DiscordMessage message = await e.Channel.GetMessageAsync(msgId);
            string author = ((DiscordMember)(message.Author)).DisplayName;
            string content = message.Content.Trim();
            DateTimeOffset date = message.CreationTimestamp;

            var quote = new Quote
            {
                Id = e.Message.Id,
                Author = author,
                Content = content,
                Date = date
            };

            _Context.Quotes.Add(quote);
            if (await _Context.SaveChangesAsync() > 0)
                await e.Channel.SendMessageAsync("Quote successfully added");
        }

        public Quote GetRandomQuote()
        {
            int count = _Context.Quotes.Count();
            int skip = new Random().Next(count+1);
            return _Context.Quotes.Skip(skip).First();
        }

        public async Task<List<Quote>> GetQuotesAsync()
        {
            return await _Context.Quotes.ToListAsync();
        }

        public List<Quote> GetQuotes(Func<Quote, bool> func)
        {
            return _Context.Quotes.Where(func).ToList();
        }
    }
}
