using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wox.Plugin.Runner
{
    class CommandLoader
    {
        readonly static string configPath = Path.Combine(
            Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Wox.Plugin.Runner" );
        readonly static string configFile = Path.Combine( configPath, "commands.json" );

        public CommandLoader()
        {
            Directory.CreateDirectory( configPath );
            if ( !File.Exists( configFile ) )
            {
                File.Create( configFile ).Close();
            }
        }

        public IEnumerable<Command> LoadCommands()
        {
            var commands = JsonConvert.DeserializeObject<IEnumerable<Command>>( File.ReadAllText( configFile ) );
            return commands ?? new List<Command>();
        }
    }
}
