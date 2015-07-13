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

        IEnumerable<Command> commands = null;

        public void Init( PluginInitContext context )
        {
            initContext = context;
            var loader = new CommandLoader();
            commands = loader.LoadCommands();
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
                                // If a command needs elevation and the user hits "No" on the UAC dialog an exception is thrown
                                // with this message. We want to ignore this exception but throw any others.
                                if ( ex.Message != "The operation was canceled by the user" )
                                    throw;
                            }
                            return true;
                        }
                } );
            results.AddRange( matches );
            return results;
        }
    }
}
