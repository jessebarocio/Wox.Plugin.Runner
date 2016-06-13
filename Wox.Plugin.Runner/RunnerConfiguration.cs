using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wox.Plugin.Runner
{
    static class RunnerConfiguration
    {
        private static IConfigurationLoader loader;
        public static IConfigurationLoader Loader 
        { 
            get
            {
                return loader ?? ( loader = new ConfigurationLoader() );
            }
            set
            {
                loader = value;
            }
        }

        private static IEnumerable<Command> commands;
        public static IEnumerable<Command> Commands
        {
            get
            {
                return commands ?? ( commands = Loader.LoadCommands() );
            }
            set
            {
                commands = value;
                Loader.SaveCommands( value );
            }
        }

        public static IEnumerable<Command> GetCommands()
        {
            return Loader.LoadCommands();
        }

        public static void SaveCommands(IEnumerable<Command> commands)
        {
            Loader.SaveCommands( commands );
        }
    }
}