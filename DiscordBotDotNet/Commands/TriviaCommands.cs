using DiscordBotDotNet.Application;
using DiscordBotDotNet.Domain;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DiscordBotDotNet.Commands
{
    public class TriviaCommands : BaseCommandModule
    {
        private readonly TriviaGame _Trivia;

        private List<string> Answers { get; set; }
        private CommandContext _CmdCtx { get; set; }
        private bool _CanAnswer { get; set; } = true;

        public TriviaCommands(TriviaGame trivia)
        {
            _Trivia = trivia;
        }

        [Command("trivia")]
        public async Task Trivia(CommandContext ctx, [RemainingText] string input)
        {
            if(input == "categories")
            {
                await ctx.RespondAsync(await _Trivia.GetTriviaCategoriesAsync());
                return;
            }

            _CmdCtx = ctx;
            string difficulty = string.Empty;
            int catId = 0;

            input = input.ToLower();

            difficulty = await ProcessDifficultyAsync(ctx, input);
            catId = await ProcessCategoryAsync(ctx, input);

            await _Trivia.StartNewGame(difficulty, catId);
            await ctx.RespondAsync("\nType `!as [number]` to answer.");

            await AskQuestion();
        }

        public Dictionary<string, ThrottleModel> Throttle { get; set; } = new Dictionary<string, ThrottleModel>();

        [Command("as")]
        public async Task ProcessAnswer(CommandContext ctx, int index)
        {

            if (!Throttle.ContainsKey(ctx.Member.DisplayName))
            {
                Throttle.Add(ctx.Member.DisplayName, new ThrottleModel {
                    Timer = new Timer(),
                    CanAnswer = true
                });

                Throttle[ctx.Member.DisplayName].Timer.Elapsed += (o, e) =>
                {
                    ctx.RespondAsync($"{ctx.Member.DisplayName} can now answer");
                    Throttle[ctx.Member.DisplayName].CanAnswer = true;
                };

                Throttle[ctx.Member.DisplayName].Timer.Interval = 4000;
                Throttle[ctx.Member.DisplayName].Timer.AutoReset = false;
            }
            
            if (!Throttle[ctx.Member.DisplayName].CanAnswer)
                return;

            Throttle[ctx.Member.DisplayName].CanAnswer = false;
            Throttle[ctx.Member.DisplayName].Timer.Start();

            if (!_CanAnswer)
                return;

            if (index < 1 || index > 4)
                return;

            string member = ctx.Member.DisplayName;

            if (_Trivia.Answer(member, Answers[index - 1]))
            {
                _CanAnswer = false;
                await _CmdCtx.RespondAsync($"Good job, {member}! **{index}** is correct.\n{_Trivia.GetScores()}");

                if (_Trivia.GetRemainingQuestion() == 0)
                {
                    await Task.Delay(4000);
                    EndGame();
                    return;
                }
                await AskQuestion();
            }
        }

        private async Task AskQuestion()
        {
            var x = _Trivia.AskQuestion();
            x.Question = ConvertHTMLCodeToString(x.Question);
            var randomizedList = RandomizedAnswers(x);
            StringBuilder builder = new StringBuilder();
            int i = 0;

            foreach (var item in randomizedList)
            {
                builder.Append($"{++i}. {item}\n");
            }

            await Task.Delay(4000);
            await _CmdCtx.RespondAsync($"{x.Question}\n{builder.ToString()}");
            
            await Task.Delay(1000);
            _CanAnswer = true;
        }

        private async Task<int> ProcessCategoryAsync(CommandContext ctx, string input)
        {
            if (input.Contains("-c"))
            {
                var index = input.IndexOf("-c") + 3;
                string num = string.Empty;
                int catId;

                while (index < input.Length && input[index] != ' ')
                {
                    num += input[index];
                    index++;
                }

                if (!int.TryParse(num, out catId))
                {
                    await ctx.RespondAsync("Could not process category id");
                    return 0;
                }

                if (catId > 32 || catId < 9)
                {
                    await ctx.RespondAsync("Incorrect category Id");
                    return 0;
                }

                return catId;
            }

            return 0;
        }

        private async Task<string> ProcessDifficultyAsync(CommandContext ctx, string input)
        {
            string difficulty = string.Empty;

            if (input.Contains("-d"))
            {
                if (!(input.Contains("easy") || input.Contains("medium") || input.Contains("hard")))
                {
                    await ctx.RespondAsync("Invalid Difficulty Setting.");
                    return null;
                }

                var index = input.IndexOf("-d") + 3;

                while (index < input.Length && input[index] != ' ')
                {
                    difficulty += input[index];
                    index++;
                }
            }

            return difficulty;
        }

        private void EndGame()
        {
            _CmdCtx.RespondAsync(_Trivia.EndGame());
        }

        private List<string> RandomizedAnswers(TriviaQuestion x)
        {
            List<string> answers = new List<string>(x.Answers);
            answers.Add(x.CorrectAnswer);

            List<string> randomized = new List<string>(answers.Count);

            Random r = new Random();
            int randomIndex;

            while (answers.Count > 0)
            {
                randomIndex = r.Next(0, answers.Count); //Choose a random object in the list
                randomized.Add(answers[randomIndex]); //add it to the new, random list
                answers.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            Answers = randomized;
            return randomized;
        }

        private string ConvertHTMLCodeToString(string x)
        {
            x = x.Replace("&#039;", "'");
            x = x.Replace("&ldquo;", "\"");
            x = x.Replace("&rsquo;", "\"");
            x = x.Replace("&rdquo;", "\"");
            x = x.Replace("&quot;", "\"");
            x = x.Replace("&hellip;", "...");
            x = x.Replace("&amp;", "&");
            x = x.Replace("&ouml;", "ö");
            return x;
        }
    }

    public class ThrottleModel
    {
        public Timer Timer { get; set; }
        public bool CanAnswer { get; set; }
    }
}
