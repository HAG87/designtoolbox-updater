using AutoUpdaterDotNET;
using DstlbxAutoUpdater.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Threading.Timer;
using System.Configuration;
using System.Collections.Specialized;

using System.IO;


namespace DstlbxAutoUpdater
{
    // The class that handles the creation of the application windows
    internal class UpdateAppContext : ApplicationContext
    {
        private string appCast = "https://raw.githubusercontent.com/HAG87/designtoolbox-release/master/versioninfo.xml";
        private Version currentVersion;

#if DEBUG
        private readonly int DelayTime = 3000;
        private readonly string appDataFile = "..\\..\\AppName.ini";
#else
        private readonly int DelayTime = 20000;
        private readonly string appDataFile = "..\\AppName.ini";
#endif

        private FormMain UIdialog;
        private static Timer UpdateTimer;

        public UpdateAppContext(bool uimode)
        {
            var appData = new IniFile(appDataFile);


            if (!appData.KeyExists("AppCast", "currentVersion"))
            {
                throw new InvalidOperationException("Missing versioninfo.xml");
            }
            if (!appData.KeyExists("Version", "productInfo"))
            {
                throw new InvalidOperationException("Missing app version");
            }
            //MessageBox.Show(new FileInfo(appDataFile).FullName);
            appCast = appData.Read("AppCast", "currentVersion");
            //MessageBox.Show(appCast);
            string posibleVersion = appData.Read("Version", "productInfo");
            currentVersion = !string.IsNullOrEmpty(posibleVersion) ? new Version(posibleVersion) : null;

            // Handle the ApplicationExit event to know when the application is exiting.
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);

            if (uimode)
            {
                ShowDialog(appCast, currentVersion);
            }
            else
            {
                UpdateTimer = new Timer(TimerAction, null, DelayTime, Timeout.Infinite);
                //UpdateTimer = new Timer(x => TimerAction(x, appCast), currentVersion, DelayTime, Timeout.Infinite);
                // SilentUpdateCheck(AppCast);
            }
        }

        //private void TimerAction(object appCast, object currentVersion)
        private void TimerAction(object state)
        {
            UpdateTimer.Dispose();
            //Console.WriteLine(currentVersion.ToString());
            // this starts the silent update check.
            SilentUpdateCheck(appCast, currentVersion);
            //SilentUpdateCheck((string)appCast, (Version)currentVersion);
        }
        private void SilentUpdateCheck(string castURL, Version currVersion, bool err = true)
        {
            // Console.WriteLine("cheking for updates");
            // Configure AutoUpdater
            AutoUpdater.AppTitle = "DesignToolBox";
            AutoUpdater.ReportErrors = err;
            AutoUpdater.UpdateMode = Mode.Normal;
            // Use a provided version number
            AutoUpdater.InstalledVersion = currVersion;
            // handle Update logic
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            // Handle exit logic
            // AutoUpdater.ApplicationExitEvent += AutoUpdater_ApplicationExitEvent;

            // Start the AutoUpdater
            AutoUpdater.Start(castURL);
        }

        private void ShowDialog(string castURL, Version currVersion)
        {
            // Create application forms
            UIdialog = new FormMain(castURL, currVersion);
            // Handle the Closed event
            UIdialog.Closed += new EventHandler(OnFormClosed);
            // UIdialog.Closing += new CancelEventHandler(OnFormClosing);

            UIdialog.Show();
        }

        //.............................................................//
        /// <summary>
        /// Open the download link.
        /// </summary>
        public static void OpenDownloadLink(string args)
        {
            if (string.IsNullOrEmpty(args))
            {
                throw new ArgumentException("message", nameof(args));
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo(args);
            Process.Start(processStartInfo);

        }

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args != null)
            {
                if (args.IsUpdateAvailable)
                {
                    DialogResult dialogResult;

                    if (args.Mandatory.Value)
                    {
                        dialogResult = CustomMessageBox.ShowMessage(
                                 string.Format(Resources.MessageMandatory, args.CurrentVersion, args.InstalledVersion),
                                 Resources.CaptionUpdate,
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
                    }
                    else
                    {
                        dialogResult = CustomMessageBox.ShowMessage(
                                string.Format(Resources.MessageNormal, args.CurrentVersion, args.InstalledVersion),
                                Resources.CaptionUpdate,
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);
                    }

                    if (dialogResult.Equals(DialogResult.Yes) || dialogResult.Equals(DialogResult.OK))
                    {
                        try
                        {
                            //You can use Download Update dialog used by AutoUpdater.NET to download the update.
                            OpenDownloadLink(args.DownloadURL);
                            Quit();
                            // if (AutoUpdater.DownloadUpdate())
                            // {
                            //     Application.Exit();
                            // }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        Quit();
                    }
                }
                else
                {
                    // No updates
                    // MessageBox.Show(Resources.MessageNoUpdate,Resources.MessageNoUpdate, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Quit();
                }
            }
            else
            {
                // CustomMessageBox.ShowMessage( Resources.MesaggeOffline, Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Quit();
            }
        }

        /*
        private void AutoUpdater_ApplicationExitEvent()
        {
            Thread.Sleep(5000);
            Application.Exit();
        }
        */
        //.............................................................//
        private void OnApplicationExit(object sender, EventArgs e)
        {
            // Quit();
        }

        private void OnFormClosing(object sender, CancelEventArgs e)
        {
            // When a form is closing, remember the form position so it
            // can be saved in the user data file.
        }

        private void OnFormClosed(object sender, EventArgs e)
        {
            // When a form is closed, decrement the count of open forms. ??? MISSING !!!
            // When the count gets to 0, exit the app by calling
            Quit();
        }

        //.............................................................//
        /// <summary>
        /// Quit the App.
        /// </summary>
        public void Quit() => Application.Exit(); //   ExitThread();
    }

    internal class Program
    {
        private static UpdateAppContext context;

        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Create the MyApplicationContext, that derives from ApplicationContext,
            // that manages when the application should exit.

            if (args != null)
            {
                foreach (var arg in args)
                {
                    if (arg.ToLower() == "silent")
                    {
                        AppRun(false);
                        return;
                    }
                }
            }
            // Run the application with the specific context.
            AppRun(true);
        }

        private static void AppRun(bool uimode)
        {
            context = new UpdateAppContext(uimode);
            Application.Run(context);
        }
    }
}