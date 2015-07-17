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