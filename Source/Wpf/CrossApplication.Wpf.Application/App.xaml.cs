﻿using System.Globalization;
using System.Threading;
using System.Windows;
using CrossApplication.Wpf.Application.Properties;

namespace CrossApplication.Wpf.Application
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SetCurrentCulture();

            new Bootstrapper().Run();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (e.ApplicationExitCode != 0)
                return;

            Settings.Default.Save();
        }

        private static void SetCurrentCulture()
        {
            if (string.IsNullOrWhiteSpace(Settings.Default.ApplicationCulture))
                return;

            var cultureInfo = new CultureInfo("de");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
    }
}