using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotDotNet.Domain
{
    public class TriviaQuestion
    {
        public string Question { get; set; }
        public List<string> Answers { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
