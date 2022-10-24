using System;
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
            var client = await GetClient();

            var channel = await RestTextChannel(client);

            var imageUrl = await GetImageUrl();

            await channel.SendMessageAsync("Daily Inspiration:");
            await channel.SendMessageAsync(imageUrl);
        }

        private static async Task<RestTextChannel> RestTextChannel(DiscordRestClient client)
        {
            var guild = (await client.GetGuildsAsync()).First(x => x.Name == "The Queen Is Dead");
            var channel = (await guild.GetTextChannelsAsync()).First(x => x.Name == "general");
            return channel;
        }

        private static async Task<string> GetImageUrl()
        {
            var httpClient = new HttpClient();
            var httpResponseMessage =
                await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://inspirobot.me/api?generate=true"));
            var imageUrl = await httpResponseMessage.Content.ReadAsStringAsync();
            return imageUrl;
        }

        private static async Task<DiscordRestClient> GetClient()
        {
            var client = new DiscordRestClient();
            var token = Environment.GetEnvironmentVariable("BOTTOKEN");
            await client.LoginAsync(TokenType.Bot, token);
            return client;
        }
    }
}