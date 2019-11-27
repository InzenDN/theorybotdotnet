using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBotDotNet.Commands
{
    public class AttackCommands : BaseCommandModule
    {
        private readonly List<string> VictoryQuotes = new List<string>
        {
            "That was super effective!",
            "Fatality.",
            "Flawless Victory.",
            "Right in the kidney!",
            "I'm sorry.",
            "Ha, ha, ha... Ha, ha, ha! Ha! Ha!",
            "What a joke!",
            "Man enough to fight with me, but not man enough to defeat me!",
            "Learn from your defeat, child.",
            "I haven't begun to use my full power. Come on, you pansy!",
            "Hey, did you hurt yourself?",
            "You're all bite and no bark, chump. This fight almost bored me to tears.",
            "For wimps like you, using my full strength is unnecessary.",
            "That's your best?",
            "Hey, you're not that bad. But you're not very good, either.",
            "I've known tougher sandbags!",
            "I saw a good training machine on an infomercial. Buy it!",
            "Don't waste my time with your weak skills, scrub!",
            "Ha-ha-ha! What's the matter? You don't like losing? Well, that's not my problem. Ha, ha, ha, ha, ha!",
            "I won't say you're weak. I'll just think it, OK?",
            "Ancient words of wisdom... \"you suck\". Ha ha ha!",
            "I hope I didn't hurt your ego too badly... Oops!",
            "Hey! Don't worry about it! You know... being weak and all!",
            "The tragedy of all losers is that they think they were on the verge of victory.",
            "*Yawn* ... Huh? It's over already? But I just woke up!",
            "How dare you even think that you are on my level of skill! Now suffer!!",
            "The reason you lost is quite simple: you're weak!",
            "Remember that one time during the fight when it looked like you might actually win? No? Me neither.",
            "Don't blame bad luck or fate. You lost because you suck.",
            "Hah, you're weak! Just like me!",
            "My dad could beat you, and he's dead!",
            "That was a 100% effort on my part! Well, actually, no... That was more like 50%.",
            "If you expected me to lose out of generosity, I'm truly sorry!",
            "How about putting up a fight next time, huh?",
        };

        [Command("slap")]
        public async Task Slap(CommandContext ctx, DiscordMember member)
        {
            int acc = new Random().Next(0, 101);

            if (acc <= 50)
            {
                if (ctx.User.Mention == member.Mention || member == null)
                {
                    await ctx.RespondAsync($"**{ctx.Member.DisplayName}** slapped themself! What a loser!");
                    return;
                }

                await ctx.RespondAsync($"**{ctx.Member.DisplayName}** slapped **{member.DisplayName}**\n\n**{ctx.Member.DisplayName}:** {VictoryQuotes[new Random().Next(0, VictoryQuotes.Count)]}");
            }
            else if (acc <= 75)
            {
                await Dodged(ctx, member);
            }
            else
            {
                await ctx.RespondAsync($"In midst of confusion, **{ctx.Member.DisplayName}** slapped themself! What a loser!");
            }
        }

        [Command("slap")]
        public async Task Slap(CommandContext ctx)
        {
            await ctx.RespondAsync($"**{ctx.Member.DisplayName}** slapped themself! What a loser!");
            return;
        }


        [Command("stab")]
        public async Task Stab(CommandContext ctx, DiscordMember member)
        {
            int acc = new Random().Next(0, 101);

            if (acc <= 50)
            {
                if (ctx.User.Mention == member.Mention)
                {
                    await ctx.RespondAsync($"**{ctx.Member.DisplayName}** committed seppuku! What a loser!");
                    return;
                }

                await ctx.RespondAsync($"**{ctx.Member.DisplayName}** stabbed **{member.DisplayName}** with a knife! :dagger::scream:\n\n**{ctx.Member.DisplayName}:** {VictoryQuotes[new Random().Next(0, VictoryQuotes.Count)]}");
            }
            else if (acc <= 75)
            {
                await Dodged(ctx, member);
            }
            else
            {
                await ctx.RespondAsync($"In midst of confusion, **{ctx.Member.DisplayName}** committed seppuku! What a loser!");
            }
        }

        private List<string> SeppukuQuotes { get; set; } = new List<string>
        {
            "Tired of your bullshit, ",
            "Losing faith in the world, ",
            "Disgusted by your shameful display, ",
            "No longer wanting to stay on this planet, ",
        };

        [Command("stab")]
        public async Task Stab(CommandContext ctx)
        {
            await ctx.RespondAsync($"{ SeppukuQuotes[new Random().Next(0, SeppukuQuotes.Count+1)] }, **{ctx.Member.DisplayName}** committed seppuku!");
        }

        [Command("shoot")]
        public async Task Shoot(CommandContext ctx, DiscordMember member)
        {
            int acc = new Random().Next(0, 101);

            if (acc <= 50)
            {
                if (ctx.User.Mention == member.Mention)
                {
                    await ctx.RespondAsync($"**{ctx.Member.DisplayName}** blew their brain out! What a loser!");
                    return;
                }

                await ctx.RespondAsync($"**{ctx.Member.DisplayName}** shot **{member.DisplayName}**! :scream::gun:\n\n**{ctx.Member.DisplayName}:** {VictoryQuotes[new Random().Next(0, VictoryQuotes.Count)]}");
            }
            else if (acc <= 75)
            {
                await Dodged(ctx, member);
            }
            else
            {
                await ctx.RespondAsync($"In midst of confusion, **{ctx.Member.DisplayName}** blew their brain out! What a loser!");
            }
        }

        [Command("shoot")]
        public async Task Shoot(CommandContext ctx)
        {
            await ctx.RespondAsync($"**{ctx.Member.DisplayName}** blew their brain out! What a loser!");
        }

        [Command("punch")]
        public async Task Punch(CommandContext ctx, DiscordMember member)
        {
            int acc = new Random().Next(0, 101);

            if (acc <= 50)
            {
                if (ctx.User.Mention == member.Mention)
                {
                    await ctx.RespondAsync($"**{ctx.Member.DisplayName}** punched their own face in! What a loser!");
                    return;
                }

                await ctx.RespondAsync($"**{ctx.Member.DisplayName}** punched **{member.DisplayName}**! :punch:\n\n**{ctx.Member.DisplayName}:** {VictoryQuotes[new Random().Next(0, VictoryQuotes.Count)]}");
            }
            else if (acc <= 75)
            {
                await Dodged(ctx, member);
            }
            else
            {
                await ctx.RespondAsync($"In midst of confusion, **{ctx.Member.DisplayName}** punched their own face in! What a loser!");
            }
        }

        [Command("punch")]
        public async Task Punch(CommandContext ctx)
        {
            await ctx.RespondAsync($"**{ctx.Member.DisplayName}** punched their own face in! What a loser!");
        }

        [Command("kick")]
        public async Task Kick(CommandContext ctx, DiscordMember member)
        {
            int acc = new Random().Next(0, 101);

            if (acc <= 50)
            {
                if (ctx.User.Mention == member.Mention)
                {
                    await ctx.RespondAsync($"**{ctx.Member.DisplayName}** kicked their own ass! What a loser!");
                    return;
                }

                await ctx.RespondAsync($"**{ctx.Member.DisplayName}** kicked **{member.DisplayName}**!\n\n**{ctx.Member.DisplayName}:** {VictoryQuotes[new Random().Next(0, VictoryQuotes.Count)]}");
            }
            else if (acc <= 75)
            {
                await Dodged(ctx, member);
            }
            else
            {
                await ctx.RespondAsync($"In midst of confusion, **{ctx.Member.DisplayName}** kicked their own ass! What a loser!");
            }
        }

        [Command("kick")]
        public async Task Kick(CommandContext ctx)
        {
            await ctx.RespondAsync($"**{ctx.Member.DisplayName}** kicked their own ass! What a loser!");
        }

        private async Task Dodged(CommandContext ctx, DiscordMember member)
        {
            var num = new Random().Next(0, 5);
            string quote = string.Empty;

            switch (num)
            {
                case 0:
                    quote = $"{member.DisplayName} dodged successfully! :middle_finger:";
                    break;
                case 1:
                    quote = $"{member.DisplayName} parried successfully! :middle_finger:";
                    break;
                case 2:
                    quote = $"{member.DisplayName} deflected successfully! :middle_finger:";
                    break;
                case 3:
                    quote = $"{member.DisplayName} blocked successfully! :middle_finger:";
                    break;
                case 4:
                    quote = $"{member.DisplayName} bullet time dodged successfully! :middle_finger:";
                    break;
            }
            
            await ctx.RespondAsync(quote);

        }
    }
}
