using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wox.Plugin.Runner.Services;

namespace Wox.Plugin.Runner.Settings
{
    class RunnerSettingsViewModel : ViewModelBase
    {
        private readonly PluginInitContext pluginContext;

        internal RunnerSettingsViewModel()
        {
            if ( IsInDesignMode )
            {
                RunnerConfiguration.Loader = new DesignTimeConfigurationLoader();
            }
            LoadCommands();
            if ( IsInDesignMode )
            {
                SelectedCommand = Commands.First();
                SelectedCommand.Shortcut = "Dirty";
            }
        }

        public RunnerSettingsViewModel( PluginInitContext context ) : this()
        {
            pluginContext = context;
        }

        private void LoadCommands()
        {
            Commands = new ObservableCollection<CommandViewModel>(
                RunnerConfiguration.Commands.Select( c => new CommandViewModel( c ) ) );
        }

        private ObservableCollection<CommandViewModel> commands;
        public ObservableCollection<CommandViewModel> Commands
        {
            get
            {
                return commands;
            }
            set
            {
                Set( () => Commands, ref commands, value );
            }
        }

        private CommandViewModel selectedCommand;
        public CommandViewModel SelectedCommand
        {
            get
            {
                return selectedCommand;
            }
            set
            {
                Set( () => SelectedCommand, ref selectedCommand, value );
            }
        }

        public bool CommandIsSelected
        {
            get
            {
                return SelectedCommand != null;
            }
        }

        private RelayCommand add;
        public RelayCommand Add
        {
            get
            {
                return add
                    ?? ( add = new RelayCommand(
                    () =>
                    {
                        var cmd = new CommandViewModel( new Command() );
                        Commands.Add( cmd );
                        SelectedCommand = cmd;
                    } ) );
            }
        }

        private RelayCommand delete;
        public RelayCommand Delete
        {
            get
            {
                return delete
                    ?? ( delete = new RelayCommand(
                    () =>
                    {
                        if ( SelectedCommand != null )
                        {
                            Commands.Remove( SelectedCommand );
                            SelectedCommand = null;
                        }
                    } ) );
            }
        }

        private RelayCommand saveChanges;
        public RelayCommand SaveChanges
        {
            get
            {
                return saveChanges
                    ?? ( saveChanges = new RelayCommand(
                    () =>
                    {
                        if ( Commands.Any( c => String.IsNullOrEmpty( c.Shortcut ) || String.IsNullOrEmpty( c.Path ) ) )
                        {
                            SimpleIoc.Default.GetInstance<IMessageService>()
                            .ShowErrorMessage( 
                                "One or more commands is missing a Shortcut or Path. Set a Shortcut and Path and try again." );
                        }
                        else
                        {
                            RunnerConfiguration.Commands = Commands.Select( c => c.GetCommand() );
                            SimpleIoc.Default.GetInstance<IMessageService>().ShowMessage( "Your changes have been saved!" );
                        }
                    } ) );
            }
        }
    }
}
