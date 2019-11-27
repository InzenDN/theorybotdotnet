using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBotDotNet
{
    public class DiceCommands : BaseCommandModule
    {
        [Command("!roll")]
        public async Task Roll(CommandContext ctx, string roll)
        {
            if (!roll.Contains("d"))
                await ctx.RespondAsync("Invalid input");

            var split = roll.Split("d");

            int numDie;
            int sizeDie;
            int.TryParse(split[0], out numDie);
            int.TryParse(split[1], out sizeDie);

            int totalValue = 0;
            List<int> values = new List<int>();

            for (int i = 0; i < numDie; i++)
            {
                int value = new Random().Next(1, sizeDie);
                values.Add(value);
                totalValue += value;
            }

            await ctx.RespondAsync($"{ctx.Member.Mention} :game_die:\n**Result:** {numDie}d{sizeDie} ({string.Join(", ", values)})\n**Total:** {totalValue}");
        }
    }
}
