using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DiscordBotDotNet.Commands
{
    public class ImagesCommands : BaseCommandModule
    {
        [Command("cat")]
        public async Task CatImage(CommandContext ctx)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync("http://shibe.online/api/cats?count=1");

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var sr = new StreamReader(stream))
                {
                    var message = await sr.ReadToEndAsync();
                    var msgObj = JsonConvert.DeserializeObject<List<string>>(message);

                    var embed = new DiscordEmbedBuilder
                    {
                        ImageUrl = msgObj[0]
                    };

                    await ctx.RespondAsync(embed: embed);
                }
            };
        }

        [Command("shiba")]
        public async Task ShibaImage(CommandContext ctx)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync("http://shibe.online/api/shibes?count=1");

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var sr = new StreamReader(stream))
                {
                    var message = await sr.ReadToEndAsync();
                    var msgObj = JsonConvert.DeserializeObject<List<string>>(message);

                    var embed = new DiscordEmbedBuilder
                    {
                        ImageUrl = msgObj[0]
                    };

                    await ctx.RespondAsync(embed: embed);
                }
            };
        }

        [Command("bird")]
        public async Task BirdImage(CommandContext ctx)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync("http://shibe.online/api/birds?count=1");

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var sr = new StreamReader(stream))
                {
                    var message = await sr.ReadToEndAsync();
                    var msgObj = JsonConvert.DeserializeObject<List<string>>(message);

                    var embed = new DiscordEmbedBuilder
                    {
                        ImageUrl = msgObj[0]
                    };

                    await ctx.RespondAsync(embed: embed);
                }
            };
        }
    }
}
