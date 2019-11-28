using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotDotNet
{
    public class DiceCommands : BaseCommandModule
    {
        private CommandContext _CmdCtx;

        [Command("!roll")]
        public async Task Roll(CommandContext ctx, string roll, params string[] flags)
        {
            bool adv = flags.Contains("-adv");
            bool dis = flags.Contains("-dis");

            if (adv && dis)
            {
                await ctx.RespondAsync("Cannot do both advantage and disadvantage.");
                return;
            }

            _CmdCtx = ctx;
            
            // Set up die
            Dice dice1 = await RollDiceAsync(roll);
            Dice dice2 = new Dice();

            if (adv ^ dis)
                dice2 = await RollDiceAsync(roll);

            // Get Total die value
            int total = dice1.TotalValue;

            if (adv && (dice1.TotalValue <= dice2.TotalValue))
                total = dice2.TotalValue;

            if (dis && (dice1.TotalValue >= dice2.TotalValue))
                total = dice2.TotalValue;

            // Begin building string
            StringBuilder builder = new StringBuilder($"{ ctx.Member.Mention } :game_die:\n**Result:** {dice1.DiceCount}d{dice1.DiceSize} ");

            // If Advantage/Disadvantage
            if (adv ^ dis)
                builder.Append($" ({dice1.TotalValue}, {dice2.TotalValue})");
            else if (!(adv || dis))
                builder.Append($" ({string.Join(", ", dice1.ValuesRolled)})");

            // Modifier
            int modifier = dice1.Modifier;
            if (dice1.Modifier != 0)
            {
                if(!(adv ^ dis))
                    builder.Append($" ({total})");

                if (roll.Contains('-'))
                    builder.Append($" - {Math.Abs(modifier)}");
                else if (roll.Contains('+'))
                    builder.Append($" + {modifier}");
            } 

            builder.Append("\n");

            // Total
            builder.Append($"**Total:** ");

            builder.Append($"{total + modifier}");

            await ctx.RespondAsync(builder.ToString());
        }

        [Command("!roll")]
        public async Task Roll(CommandContext ctx, string roll)
        {
            _CmdCtx = ctx;

            Dice dice = await RollDiceAsync(roll);

            await ctx.RespondAsync($"{ctx.Member.Mention} :game_die:\n**Result:** {dice.DiceCount}d{dice.DiceSize} ({string.Join(", ", dice.ValuesRolled)})\n**Total:** {dice.TotalValue}");
        }

        private async Task<Dice> RollDiceAsync(string roll)
        {
            if (!roll.Contains("d"))
                await _CmdCtx.RespondAsync("Invalid input");

            // Dice Roll
            var split = roll.Split('d', '-', '+');

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

            // Modifier
            int modifier = 0;

            if (roll.Contains('-') || roll.Contains('+'))
            {
                if (roll.Contains("-"))
                {
                    if (!int.TryParse('-' + roll.Split("-")[1], out modifier))
                        await _CmdCtx.RespondAsync("Invalid modifier value.");
                }
                else if (roll.Contains('+'))
                {
                    if (!int.TryParse(roll.Split("+")[1], out modifier))
                        await _CmdCtx.RespondAsync("Invalid modifier value.");
                }
            }

            Dice dice = new Dice
            {
                TotalValue = totalValue,
                ValuesRolled = values,
                DiceCount = numDie,
                DiceSize = sizeDie,
                Modifier = modifier
            };

            return dice;
        }
    }

    public class Dice
    {
        public int Modifier { get; set; }
        public int DiceCount { get; set; }
        public int DiceSize { get; set; }
        public int TotalValue { get; set; }
        public List<int> ValuesRolled { get; set; }
    }
}
