using DiscordBotDotNet.DataTransferObjects;
using DiscordBotDotNet.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotDotNet.Application
{
    public class TriviaGame
    {
        private readonly HttpRequest _Http;
        private static Queue<TriviaQuestion> _Questions;
        private static Dictionary<string, int> _Scores;

        public TriviaGame(HttpRequest http)
        {
            _Http = http;
        }

        public async Task StartNewGame(string difficulty, int categoryId)
        {
            string catId = string.Empty;

            if(categoryId + 8 >= 9 || categoryId  + 8 <= 32)
            {
                catId = categoryId.ToString();
            }

            Uri uri = new Uri($"https://opentdb.com/api.php?amount=10&difficulty={difficulty}&type=multiple&category={categoryId}");
            var Trivia = await _Http.GetAsync<TriviaResponseDTO>(uri);
            _Scores = new Dictionary<string, int>();
            _Questions = new Queue<TriviaQuestion>();

            foreach (var item in Trivia.Results)
            {
                var q = new TriviaQuestion
                {
                    Question = item.Question,
                    Answers = item.IncorrectAnswers,
                    CorrectAnswer = item.CorrectAnswer
                };

                _Questions.Enqueue(q);
            }
        }

        public string EndGame()
        {
            _Questions.Clear();
            return GetWinners();
        }

        private string GetWinners()
        {
            var scorers = GetTopScorers();

            StringBuilder builder = new StringBuilder("```diff\n+ The Winner(s) +\n\n");

            builder.Append(string.Join(", ", scorers));

            builder.Append($"\n\nWith {_Scores.Values.Max()} points!");

            builder.Append("\n```");

            return builder.ToString();
        }

        public bool Answer(string member, string answer)
        {
            if (!_Scores.ContainsKey(member))
                _Scores.Add(member, 0);

            if (answer != _Questions.Peek().CorrectAnswer)
                return false;

            _Questions.Dequeue();
            _Scores[member]++;

            return true;
        }

        public int GetRemainingQuestion()
        {
            return _Questions.Count;
        }

        public string GetScores()
        {
            StringBuilder builder = new StringBuilder("```diff\n+ -Scoreboard- +\n");

            foreach (var item in _Scores)
            {
                builder.Append($"{item.Key}: {item.Value}\n");
            }

            var leaders = GetTopScorers();

            builder.Append($"\n+ Top Scorer(s)\n");
            builder.Append($"{string.Join(", ", leaders)}");
            builder.Append("\n```");

            return builder.ToString();
        }

        private List<string> GetTopScorers()
        {
            var highestScore = _Scores.Values.Max();
            var leaders = _Scores.Where(x => x.Value == highestScore).Select(x => x.Key).ToList();
            return leaders;
        }

        public TriviaQuestion AskQuestion()
        {
            return _Questions.Peek();
        }

        public async Task<string> GetTriviaCategoriesAsync()
        {
            Uri uri = new Uri("https://opentdb.com/api_category.php");
            var categories = await _Http.GetAsync<TriviaCategoriesResponseDto>(uri);

            StringBuilder builder = new StringBuilder("```diff\n+ Categories (Id)\n");

            foreach (var item in categories.TriviaCategories)
            {
                builder.Append($"{item.Id-8} - {item.Name}\n");
            }

            builder.Append("```");

            return builder.ToString();
        }
    }
}
