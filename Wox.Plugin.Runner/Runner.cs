using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Wox.Plugin.Runner.Services;
using Wox.Plugin.Runner.Settings;

namespace Wox.Plugin.Runner
{
    public class Runner : IPlugin, ISettingProvider
    {
        PluginInitContext initContext;

        IEnumerable<Command> commands = null;

        public void Init( PluginInitContext context )
        {
            if ( !SimpleIoc.Default.IsRegistered<IMessageService>() )
            {
                SimpleIoc.Default.Register<IMessageService>( () => new MessageService() );
            }
            initContext = context;
            commands = RunnerConfiguration.GetCommands();
        }

        public List<Result> Query( Query query )
        {
            var results = new List<Result>();
            var commandName = query.ActionParameters[0];
            var matches = commands.Where( c => c.Shortcut.StartsWith( commandName ) ).Select( c => new Result()
                {
                    Title = c.Description,
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

        public Control CreateSettingPanel()
        {
            return new RunnerSettings( new RunnerSettingsViewModel( initContext ) );
        }
    }
}
