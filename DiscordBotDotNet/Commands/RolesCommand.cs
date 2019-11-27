using DiscordBotDotNet.Persistence;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotDotNet.Commands
{
    public class RolesCommand : BaseCommandModule
    {
        private readonly ApplicationDbContext _Context;

        public RolesCommand(ApplicationDbContext context)
        {
            _Context = context;
        }
        
        [Command("roles")]
        public async Task CanSetRoles(CommandContext ctx, string flag, DiscordRole role)
        {
            foreach (var item in ctx.Member.Roles)
            {
                if (item.CheckPermission(Permissions.Administrator) == PermissionLevel.Allowed)
                {
                    await SetRoles(ctx, flag, role);
                    return;
                }

                if (item.CheckPermission(Permissions.ManageRoles) == PermissionLevel.Allowed)
                {
                    await SetRoles(ctx, flag, role);
                    return;
                }
            }

            await ctx.RespondAsync("Insufficient permissions");
        }

        private async Task SetRoles(CommandContext ctx, string flag, DiscordRole role)
        {
            if (flag == "add" || flag == "-a")
            {
                _Context.PublicGuildRoles.Add(role);
                if (await _Context.SaveChangesAsync() > 0)
                    await ctx.RespondAsync($"{role.Name} added successfully.");
                else
                    await ctx.RespondAsync($"Failed to add {role.Name}.");
            }
            else if (flag == "remove" || flag == "-rm")
            {
                _Context.PublicGuildRoles.Remove(role);
                if (await _Context.SaveChangesAsync() > 0)
                    await ctx.RespondAsync($"{role.Name} removed successfully.");
                else
                    await ctx.RespondAsync($"Failed to remove {role.Name}.");
            }
        }

        [Command("roles")]
        public async Task SetRoles(CommandContext ctx, string flag)
        {
            if (flag == "-pl")
            {
                var roles = await _Context.PublicGuildRoles.ToListAsync();
                StringBuilder roleList = new StringBuilder();

                foreach (var role in roles)
                {
                    roleList.Append($"• {role.Name}\n");
                }

                await ctx.RespondAsync($"```diff\n+ Public Roles Listing:\n{(roleList.Length > 0 ? roleList.ToString() : "Empty")}\n```");
            }
        }

        [Command("roles")]
        public async Task GetRoles(CommandContext ctx)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("You are in:\n");

            foreach (var item in ctx.Member.Roles)
            {
                builder.Append($"• {item.Name}\n");
            }

            await ctx.RespondAsync(builder.ToString());
        }

        [Command("role")]
        public async Task SetRole(CommandContext ctx, string flag, DiscordRole role)
        {
            if (flag == "add" || flag == "-a")
            {
                await ctx.Member.GrantRoleAsync(role);
                await ctx.RespondAsync($"Successfully added to **{role.Name}**");
            }
            else if (flag == "remove" || flag == "-rm")
            {
                await ctx.Member.RevokeRoleAsync(role);
                await ctx.RespondAsync($"Successfully removed **{role.Name}** from roles");
            }
            else
                await ctx.RespondAsync("Invalid command input.");
        }
    }
}
