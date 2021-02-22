using AutoUpdaterDotNET;
using DstlbxAutoUpdater.Properties;
using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace DstlbxAutoUpdater
{
    public partial class FormMain : Form
    {
        private readonly string appCast;

        public FormMain(string castURL)
        {
            InitializeComponent();
            labelVersion.Text = string.Format(Resources.CurrentVersion, Assembly.GetEntryAssembly().GetName().Version);
            appCast = castURL ?? throw new ArgumentNullException(nameof(castURL));
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //Uncomment below lines to handle parsing logic of non XML AppCast file.

            //AutoUpdater.ParseUpdateInfoEvent += AutoUpdaterOnParseUpdateInfoEvent;
            //AutoUpdater.Start("https://rbsoft.org/updates/AutoUpdaterTest.json");

            //Uncomment below line to run update process using non administrator account.

            //AutoUpdater.RunUpdateAsAdmin = false;

            //Uncomment below line to see russian version
            //Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("ru");

            //If you want to open download page when user click on download button uncomment below line.
            AutoUpdater.OpenDownloadPage = true;

            //Don't want to show Remind Later button then uncomment below line.
            AutoUpdater.ShowRemindLaterButton = false;

            //Don't want user to select remind later time in AutoUpdater notification window then uncomment 3 lines below so default remind later time will be set to 2 days.

            //AutoUpdater.LetUserSelectRemindLater = false;
            //AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Minutes;
            //AutoUpdater.RemindLaterAt = 1;

            //Want to show custom application title then uncomment below line.
            AutoUpdater.AppTitle = "DesignToolBox";

            //Want to show errors then uncomment below line.
            AutoUpdater.ReportErrors = true;

            //Want to handle how your application will exit when application finished downloading then uncomment below line.
            AutoUpdater.ApplicationExitEvent += AutoUpdater_ApplicationExitEvent;

            //Want to use XML and Update file served only through Proxy.
            //var proxy = new WebProxy("localproxyIP:8080", true) {Credentials = new NetworkCredential("domain\\user", "password")};
            //AutoUpdater.Proxy = proxy;

            //Want to check for updates frequently then uncomment following lines.

            //System.Timers.Timer timer = new System.Timers.Timer
            //{
            //    Interval = 2 * 60 * 1000,
            //    SynchronizingObject = this
            //};
            //timer.Elapsed += delegate
            //{
            //    AutoUpdater.Start(appCast);
            //};
            //timer.Start();

            //Uncomment following lines to provide basic authentication credentials to use.

            //BasicAuthentication basicAuthentication = new BasicAuthentication("myUserName", "myPassword");
            //AutoUpdater.BasicAuthXML = AutoUpdater.BasicAuthDownload = basicAuthentication;

            //Want to change update form size then uncomment below line.

            //AutoUpdater.UpdateFormSize = new System.Drawing.Size(800, 600);
            this.Focus();
            this.Activate();
        }

        private void AutoUpdater_ApplicationExitEvent()
        {
            Text = @"Closing application...";
            Thread.Sleep(5000);
            Application.Exit();
        }


        //private void AutoUpdaterOnParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        //{

        //    dynamic json = JsonConvert.DeserializeObject(args.RemoteData);
        //    args.UpdateInfo = new UpdateInfoEventArgs
        //    {
        //        CurrentVersion = json.version,
        //        ChangelogURL = json.changelog,
        //        Mandatory = json.mandatory,
        //        DownloadURL = json.url
        //    };
        //}

        private void ButtonCheckForUpdate_Click(object sender, EventArgs e)
        {
            // Uncomment below lines to select download path where update is saved.
            // FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            // if (folderBrowserDialog.ShowDialog().Equals(DialogResult.OK))
            // {
            //     AutoUpdater.DownloadPath = folderBrowserDialog.SelectedPath;
            //     AutoUpdater.Mandatory = true;
            //     AutoUpdater.Start(appCast);
            // }
            // AutoUpdater.Mandatory = true;
            // AutoUpdater.UpdateMode = Mode.Forced;
            AutoUpdater.Start(appCast);
        }
    }
}
