using System.Collections.Generic;

namespace DiscordBotDotNet.DataTransferObjects
{
    public class TriviaResponseDTO
    {
        public int ResponseCode { get; set; }
        public List<Result> Results { get; set; }

        public class Result
        {
            public string Category { get; set; }
            public string Type { get; set; }
            public string Difficulty { get; set; }
            public string Question { get; set; }
            public string CorrectAnswer { get; set; }
            public List<string> IncorrectAnswers { get; set; }
        }
    }

    
}
