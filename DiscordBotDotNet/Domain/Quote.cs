using System;

namespace DiscordBotDotNet.Domain
{
    public class Quote
    {
        public ulong Id { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
    }
}
