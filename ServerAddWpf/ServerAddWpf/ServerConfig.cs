using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

using EnvDTE;
namespace ServerAddWpf
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class ServerConfig
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("3ae701ba-60f9-4ca9-b95d-d8ec250060ba");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConfig"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private ServerConfig(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            //var menuItem = new MenuCommand(this.temp, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public  ServerConfig()
        {

        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static ServerConfig Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in ServerConfig's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new ServerConfig(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            ServerWindow wnd = new ServerWindow();
            setActiveFile_inSetting();
            //wnd.Show();
            wnd.ShowDialog();
           /*
            string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            string title = "ServerConfig";

            // Show a message box to prove we were here
            VsShellUtilities.ShowMessageBox(
                this.package,
                message,
                title,
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
           */
        }

        

        private IServiceProvider ServiceProvider1
        {
            get
            {
                return this.package;
            }
        }

        //public String file_name = GetActiveDocumentFilePath(ServiceProvider1);
        public  String get_current_active_file()
        {
            return GetActiveDocumentFilePath(ServiceProvider1);
        }

        private void setActiveFile_inSetting()
        {
            //TextViewSelection selection = GetSelection(ServiceProvider1);
            //string activeDocumentPath = GetActiveDocumentFilePath(ServiceProvider1);
            String message = GetActiveDocumentFilePath(ServiceProvider1);
            Properties.Settings.Default.activeFileName = message;
            Properties.Settings.Default.Save();


        }


        private string GetActiveDocumentFilePath(IServiceProvider serviceProvider)
        {
            EnvDTE80.DTE2 applicationObject = serviceProvider.GetService(typeof(DTE)) as EnvDTE80.DTE2;
            if (applicationObject == null)
                return "-1";
            if (applicationObject.ActiveDocument == null)
                return "-1";
            String fileName = applicationObject.ActiveDocument.FullName;
            //return applicationObject.ActiveDocument.FullName;
            if (fileName == null)
                return "-1";
            else
                return fileName;

        }
    }
}
