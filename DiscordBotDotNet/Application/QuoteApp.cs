using DiscordBotDotNet.Domain;
using DiscordBotDotNet.Persistence;
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

        public QuoteApp(ApplicationDbContext context)
        {
            _Context = context;
        }

        public async Task AddQuote(string author, string content)
        {
            var quote = new Quote
            {
                Author = author,
                Content = content
            };

            _Context.Quotes.Add(quote);
            await _Context.SaveChangesAsync();
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
