using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SIVA.Core.JsonFiles;

namespace SIVA.Core.Bot.Internal.DataMigrate
{
    public class Program
    {
        private static ConsoleColor _color;
        internal static void MigratorMain(string[] args)
        {
            //data/UAccounts.json
            _color = Console.ForegroundColor;
            Console.WriteLine("SIVA Data Migration Utility");
            Console.WriteLine();
            Console.Write("Starting engine...");
            Database.SetupStorage();
            WriteOk();
            Console.Write("Loading Guild Configs...");
            var guildConfigs = JsonConvert.DeserializeObject<List<Guild>>(File.ReadAllText("data/GuildConfigs.json"));
            WriteOk();
            int guildCount = 0;
            foreach (var guild in guildConfigs)
            {
                Console.Write("Migrating "+guild.ServerId+"...");
                Database.I.Guilds.Add(guild);
                WriteOk();
                guildCount++;
            }
            Console.Write("Loading User Accounts...");
            var userAccounts = JsonConvert.DeserializeObject<List<UserAccount>>(File.ReadAllText("data/UAccounts.json"));
            WriteOk();
            int userCount = 0;
            foreach (var user in userAccounts)
            {
                Console.Write("Migrating "+user.Id+"...");
                Database.I.Users.Add(user);
                WriteOk();
                userCount++;
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Migration Report");
            Console.WriteLine("================");
            Console.WriteLine("Successfully migrated "+userCount+" users...");
            Console.WriteLine("Successfully migrated "+guildCount+" guilds...");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }

        static void WriteOk()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("OK");
        }
    }
}