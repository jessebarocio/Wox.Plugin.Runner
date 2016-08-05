using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wox.Plugin.Runner
{
    interface IConfigurationLoader
    {
        IEnumerable<Command> LoadCommands();
        void SaveCommands( IEnumerable<Command> commands );
    }

    class ConfigurationLoader : IConfigurationLoader
    {
        readonly static string configPath = Path.Combine(
            Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Wox.Plugin.Runner" );
        readonly static string configFile = Path.Combine( configPath, "commands.json" );

        public ConfigurationLoader()
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

        public void SaveCommands( IEnumerable<Command> commands )
        {
            File.WriteAllText( configFile, JsonConvert.SerializeObject( commands ) );
        }
    }

    class DesignTimeConfigurationLoader : IConfigurationLoader
    {

        public IEnumerable<Command> LoadCommands()
        {
            return new List<Command>()
            {
                new Command
                {
                    Description = "Sample Command 1",
                    Shortcut = "shortcut1",
                    Path = @"C:\mycommand1.exe",
                    WorkingDirectory = @"C:\workpath1"
                },
                new Command
                {
                    Description = "Sample Command 2",
                    Shortcut = "shortcut2",
                    Path = @"C:\mycommand2.exe",
                    WorkingDirectory = @"C:\workpath2"
                }
            };
        }

        public void SaveCommands( IEnumerable<Command> commands )
        {
            throw new NotImplementedException();
        }
    }
}
