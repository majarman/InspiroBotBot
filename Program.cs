using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;

namespace InspiroBotBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var token = args[0];
            var client = await GetClient(token);

            var channels = await GetRestTextChannelsAsync(client);

            foreach (var channel in channels)
            {
                var imageUrl = await GetImageUrl();
                await channel.SendMessageAsync("Daily Inspiration:");
                await channel.SendMessageAsync(imageUrl);
            }
        }

        private static async Task<IEnumerable<RestTextChannel>> GetRestTextChannelsAsync(DiscordRestClient client)
        {
            var allGuildsAndChannels = GetGuildsAndChannels();
            
            var allRestTextChannels = new List<RestTextChannel>();
            
            foreach (var (guildName, channelName) in allGuildsAndChannels)
            {
                var guild = (await client.GetGuildsAsync()).First(x => x.Name == guildName);
                var channel = (await guild.GetTextChannelsAsync()).First(x => x.Name == channelName);
                allRestTextChannels.Add(channel);
            }
            
            return allRestTextChannels;
        }

        private static Dictionary<string, string> GetGuildsAndChannels()
        {
            return new Dictionary<string, string>
            {
                {"WumboServer", "wumbochat"},
                // {"The Queen Is Dead", "general"},
            };
        }

        private static async Task<string> GetImageUrl()
        {
            var httpClient = new HttpClient();
            var httpResponseMessage =
                await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://inspirobot.me/api?generate=true"));
            var imageUrl = await httpResponseMessage.Content.ReadAsStringAsync();
            return imageUrl;
        }

        private static async Task<DiscordRestClient> GetClient(string token)
        {
            var client = new DiscordRestClient();
            await client.LoginAsync(TokenType.Bot, token);
            return client;
        }
    }
}