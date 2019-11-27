using System;
using System.Threading.Tasks;

namespace DiscordBotDotNet
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Startup.RunAsync();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
