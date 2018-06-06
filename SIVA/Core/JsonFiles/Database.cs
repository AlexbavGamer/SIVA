using Ultz.BeagleFramework;
using Ultz.BeagleFramework.Common;
using Ultz.BeagleFramework.Common.Models;
using Ultz.BeagleFramework.SQLite;

namespace SIVA.Core.JsonFiles
{
    public class Database : DataStore
    {
        public static DataContext<Database> Context { get; private set; }
        public static Database I => Context.Store;

        public static void SetupStorage()
        {
            Context = Beagle.CreateContext<Database>(new SqliteEngine("Filename=./data/siva.db"));
            
        }
        public Table<Guild> Guilds { get; set; }
        public Table<UserAccount> Users { get; set; }
    }
}