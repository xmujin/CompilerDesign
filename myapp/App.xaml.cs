using CefSharp.Wpf;
using CefSharp;
using System.Configuration;
using System.Data;
using System.Windows;


namespace myapp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var settings = new CefSettings();
            // Optional: Set any settings you need here
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Cef.Shutdown();
            base.OnExit(e);
        }
    }

}
