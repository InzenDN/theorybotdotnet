using DiscordBotDotNet.Application;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DiscordBotDotNet
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Startup.ConfigureAsync();
            await Startup.RunAsync();
        }
    }
}
