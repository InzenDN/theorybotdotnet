using DiscordBotDotNet.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotDotNet.DataTransferObjects
{
    public class TriviaCategoriesResponseDto
    {
        public List<TriviaCategory> TriviaCategories { get; set; }
    }
}
