﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SIVA.Core.JsonFiles;
using SIVA.Core.Modules;

namespace SIVA.Core.Bot
{
    public class ScarszDebug : SivaModule
    {
        internal static string CreateDebug(string config)
        {
            var files = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "1-Info.txt",
                    new Dictionary<string, string>
                    {
                        {
                            "content",
                            "Thanks to Scarsz lol#4227 on Discord for this tool. Without it, we wouldn't be able to do this!\n\nCheck out his stuff here:\nGitHub: https://github.com/Scarsz\nDiscord: https://discord.gg/WdFa6gc\nTwitter: https://twitter.com/ScarszRawr\n\nSupport Server: https://discord.gg/prR9Yjq\nWebsite: https://greem.xyz\nSource Code: http://code.greem.xyz/SIVA-Developers/SIVA"
                        },
                        {"description", "Thanks Scarsz for this amazing utility!"}
                    }
                },
                {
                    "2-Server.conf",
                    new Dictionary<string, string>
                    {
                        {"content", $"{config}"},
                        {"description", "Server config for debug purposes."}
                    }
                }
            };

            var payload = new
            {
                description = "Discord server settings for support.",
                files
            };
            var payloadJson = JsonConvert.SerializeObject(payload);

            var httpClient = new RestClient("https://debug.scarsz.me") {UserAgent = "SIVA/V1.5.1"};
            var req = new RestRequest("post", Method.POST);
            req.AddHeader("Content-Type", "application/json");
            req.RequestFormat = DataFormat.Json;
            req.Parameters.Clear();
            req.AddParameter("application/json", payloadJson, ParameterType.RequestBody);
            var resJson = httpClient.Execute(req);
            var res = (JObject) JsonConvert.DeserializeObject(resJson.Content);
            return res.GetValue("url").ToString().Replace("scarsz.me", "greem.xyz");
        }

        [Command("ForceDebug")]
        [RequireOwner]
        public async Task CreateDebugToUrl(ulong serverId)
        {
            await Helpers.SendMessage(Context,
                Helpers.CreateEmbed(Context, CreateDebug(CreateConfigString(GuildConfig.GetOrCreateConfig(serverId)))));
        }

        [Command("Debug")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task CreateDebugToUrl()
        {
            await Helpers.SendMessage(Context,
                Helpers.CreateEmbed(Context,
                    $"{CreateDebug(CreateConfigString(GuildConfig.GetOrCreateConfig(Context.Guild.Id)))}\n\nTake this to the SIVA server for support. Join the server [here](https://greem.xyz/SIVA)."));
        }

        private static string CreateConfigString(Guild config)
        {
            // TODO: Please oh god please rewrite this
            var customCommands = "";
            var blacklistedWords = "";
            var selfRoles = "";
            var antilinkIgnoredChannels = "";
            foreach (var command in config.CustomCommands) customCommands += $"{command.Key}: {command.Value} || ";

            foreach (var word in config.Blacklist) blacklistedWords += $"{word}, ";

            foreach (var role in config.SelfRoles) selfRoles += $"{role}, ";

            foreach (var channelId in config.AntilinkIgnoredChannels) antilinkIgnoredChannels += $"{channelId}, ";

            var parsedConfig = $"ServerId: {config.ServerId}\n" +
                               $"ServerOwner: {config.GuildOwnerId}\n" +
                               "\n\n" +
                               "Lists:\n" +
                               $"    AntilinkIgnoredChannels: {antilinkIgnoredChannels}\n" +
                               $"    SelfAssignableRoles: {selfRoles}\n" +
                               $"    Blacklist: {blacklistedWords}\n" +
                               $"    CustomCommands: {customCommands}\n" +
                               "\n" +
                               "Support:\n" +
                               $"    CanCloseOwnTicket: {config.CanCloseOwnTicket}\n" +
                               $"    SupportChannelName: {config.SupportChannelName}\n" +
                               $"    SupportChannelId: {config.SupportChannelId}\n" +
                               $"    SupportRole: {config.SupportRole}\n" +
                               "\n" +
                               "Moderation:\n" +
                               $"    AntiLink: {config.Antilink}\n" +
                               $"    MassPingChecks: {config.MassPengChecks}\n" +
                               "\n" +
                               "Welcome/Leaving:\n" +
                               $"    WelcomeMessage: {config.WelcomeMessage}\n" +
                               $"    LeavingMessage: {config.LeavingMessage}\n" +
                               $"    WelcomeColorRGB: {config.WelcomeColour1}, {config.WelcomeColour2}, {config.WelcomeColour2}\n" +
                               $"    WelcomeChannel: {config.WelcomeChannel}\n" +
                               "\n" +
                               "Miscellaneous:\n" +
                               $"    TruthOrDareEnabled: {config.IsTodEnabled}\n" +
                               $"    IsServerLoggingEnabled: {config.IsServerLoggingEnabled}\n" +
                               $"    ServerLoggingChannel: {config.ServerLoggingChannel}\n" +
                               $"    ModeratorRole: {config.ModRole}\n" +
                               $"    AdminRole: {config.AdminRole}\n" +
                               $"    CommandPrefix {config.CommandPrefix}\n" +
                               "\n" +
                               "Donator:\n" +
                               $"    CommandEmbedColourRGB: {config.EmbedColour1}, {config.EmbedColour2}, {config.EmbedColour3}\n" +
                               "    (More coming for donators soon™)";

            return parsedConfig;
        }
    }
}