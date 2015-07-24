using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wox.Plugin.Runner.Settings;

namespace Wox.Plugin.Runner.Configurator
{
    class MainWindowViewModel : ViewModelBase
    {
        private RunnerSettingsViewModel settingsViewModel;
        public RunnerSettingsViewModel SettingsViewModel
        {
            get
            {
                return settingsViewModel ?? (settingsViewModel = new RunnerSettingsViewModel());
            }
        }
    }
}
