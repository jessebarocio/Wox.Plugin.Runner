using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Wox.Plugin.Runner.Services;
using Wox.Plugin.Runner.Settings;

namespace Wox.Plugin.Runner
{
    public class Runner : IPlugin, ISettingProvider
    {
        PluginInitContext initContext;
        bool isGlobal;

        public void Init( PluginInitContext context )
        {
            if ( !SimpleIoc.Default.IsRegistered<IMessageService>() )
            {
                SimpleIoc.Default.Register<IMessageService>( () => new MessageService() );
            }
            initContext = context;
            isGlobal = context.CurrentPluginMetadata.ActionKeywords.Contains(Plugin.Query.GlobalPluginWildcardSign);
        }

        public List<Result> Query( Query query )
        {
            var results = new List<Result>();
            if (query.Terms.Length < 2 && !this.isGlobal) return results;

            var commandName = query.Terms[isGlobal ? 0 : 1];
            var matches = RunnerConfiguration.Commands.Where( c => c.Shortcut.StartsWith( commandName ) )
                .Select( c => new Result()
                {
                    Score = int.MaxValue / 2,
                    Title = c.Description,
                    Action = e => RunCommand( e, query, c )
                } );
            results.AddRange( matches );
            return results;
        }

        public Control CreateSettingPanel()
        {
            return new RunnerSettings( new RunnerSettingsViewModel( initContext ) );
        }

        private bool RunCommand( ActionContext e, Query query, Command command )
        {
            try
            {
                var args = GetProcessArguments( command, query );
                Process.Start( args.FileName, args.Arguments );
            }
            catch ( Win32Exception w32Ex )
            {
                // If a command needs elevation and the user hits "No" on the UAC dialog an exception is thrown
                // with this message. We want to ignore this exception but throw any others.
                if ( w32Ex.Message != "The operation was canceled by the user" )
                    throw;
            }
            catch ( FormatException )
            {
                SimpleIoc.Default.GetInstance<IMessageService>().ShowErrorMessage(
                    "There was a problem. Please check the arguments format for the command." );
            }
            return true;
        }

        private ProcessArguments GetProcessArguments( Command c, Query q )
        {
            string argString = String.Empty;
            if ( !String.IsNullOrEmpty( c.ArgumentsFormat ) )
            {
                var arguments = q.Terms.ToList();
                //arguments.RemoveAt( 0 );
                if ( !isGlobal )
                    arguments.RemoveAt( 0 );
                if ( arguments.Count > 0 )
                {
                    argString = String.Format( c.ArgumentsFormat, arguments.ToArray() );
                }
                else
                    argString = String.Empty;
            }
            return new ProcessArguments
            {
                FileName = c.Path,
                Arguments = argString
            };
        }

        class ProcessArguments
        {
            public string FileName { get; set; }
            public string Arguments { get; set; }
        }
    }
}
