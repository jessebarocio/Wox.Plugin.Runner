using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Wox.Plugin.Runner.Services;

namespace Wox.Plugin.Runner.Configurator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SimpleIoc.Default.Register<IMessageService>(() => new MessageService());
        }
    }
}
