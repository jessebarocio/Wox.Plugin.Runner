using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Wox.Plugin.Runner.Services
{
    interface IMessageService
    {
        void ShowMessage( string message );
        void ShowErrorMessage( string message );
    }

    class MessageService : IMessageService
    {

        public void ShowMessage( string message )
        {
            MessageBox.Show( message, "Runner for Wox", MessageBoxButton.OK, MessageBoxImage.Information );
        }

        public void ShowErrorMessage( string message )
        {
            MessageBox.Show( message, "Runner for Wox", MessageBoxButton.OK, MessageBoxImage.Error );
        }
    }
}
