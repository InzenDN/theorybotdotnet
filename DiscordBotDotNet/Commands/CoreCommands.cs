using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBotDotNet.Commands
{
    public class CoreCommands : BaseCommandModule
    {
        private readonly IWebDriver _Driver;
        private List<string> Fortunes { get; set; }

        public CoreCommands()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless --disable-gpu");
            _Driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), chromeOptions);
            LoadFortunes().GetAwaiter().GetResult();
        }

        private async Task LoadFortunes()
        {
            Fortunes = new List<string>();

            using (Stream stream = new FileStream("fortunes.json", FileMode.Open))
            using (StreamReader sr = new StreamReader(stream))
            {
                string line;
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    Fortunes.Add(line);
                }
            }
        }

        [Command("hi")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync($"👋 Hi, {ctx.Member.DisplayName}!\nWelcome to {ctx.Guild.Name}");
        }

        [Command("norby")]
        public async Task NorbySays(CommandContext ctx)
        {
            await ctx.RespondAsync("I enjoy the sight of human on their knees.");
        }

        [Command("trout")]
        public async Task SlapTrout(CommandContext ctx, DiscordMember member)
        {
            await ctx.RespondAsync($"**{ctx.Member.DisplayName}** slapped **{member.DisplayName}** around a bit with a large trout.");
        }

        [Command("channelid")]
        public async Task GetChannelId(CommandContext ctx)
        {
            await ctx.RespondAsync($"{ctx.Channel.Id}");
        }

        [Command("joke")]
        public async Task Joke(CommandContext ctx)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync("https://icanhazdadjoke.com/");

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var sr = new StreamReader(stream))
                {
                    var message = await sr.ReadToEndAsync();
                    var msgObj = JsonConvert.DeserializeObject<JokeResponseDTO>(message);
                    await ctx.RespondAsync(msgObj.Joke);
                }
            };
        }

        [Command("urban")]
        public async Task UrbanAsync(CommandContext ctx, [RemainingText] string keywords)
        {
            if (!ctx.Channel.IsNSFW)
            {
                await ctx.RespondAsync("Only for NSFW channels.");
                return;
            }

            _Driver.Url = $"https://www.urbandictionary.com/define.php?term={keywords}";

            var wait = new WebDriverWait(_Driver, new TimeSpan(0, 0, 30));

            var node1 = wait.Until(x => x.FindElement(By.CssSelector("div.meaning")));
            var node2 = wait.Until(x => x.FindElement(By.CssSelector("div.example")));

            await ctx.RespondAsync($" \n**-- {keywords.Trim()} --**" +
                $"\n{node1.Text.Replace("&quot;", "\"").Replace("&apos;", "\'")}" +
                $"\n\n**Example:**\n{node2.Text.Replace("&quot;", "\"").Replace("&apos;", "\'")}");
        }

        [Command("fortune")]
        public async Task FortuneCookie(CommandContext ctx)
        {
            await ctx.RespondAsync(Fortunes[new Random().Next(0, Fortunes.Count+1)]);
        }
    }
}
