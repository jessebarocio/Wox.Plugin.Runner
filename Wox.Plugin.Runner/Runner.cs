using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Wox.Plugin.Runner
{
    public class Runner : IPlugin
    {
        PluginInitContext initContext;

        IEnumerable<Command> commands = new List<Command>()
        {
            new Command
            {
                Description = "Visual Studio",
                Shortcut = "vs",
                Path = @"C:\config\shortcuts\VisualStudio.lnk"
            },
            new Command
            {
                Description = "Visual Studio (Administrator)",
                Shortcut = "vsadmin",
                Path = @"C:\config\shortcuts\VisualStudioAdmin.lnk"
            }
        };

        public void Init( PluginInitContext context )
        {
            initContext = context;
        }

        public List<Result> Query( Query query )
        {
            var results = new List<Result>();
            var commandName = query.ActionParameters[0];
            var matches = commands.Where( c => c.Shortcut.StartsWith( commandName ) ).Select( c => new Result()
                {
                    Title = c.Description,
                    SubTitle = c.Path,
                    IcoPath = "Images\\globe.png",
                    Action = e =>
                        {
                            try
                            {
                                Process.Start( c.Path );
                            }
                            catch ( Win32Exception ex )
                            {
                                if ( ex.Message == "The operation was canceled by the user" )
                                {
                                    // do nothing
                                }
                                else
                                {
                                    throw;
                                }
                            }
                            return true;
                        }
                } );
            results.AddRange( matches );
            return results;
        }
    }
}
