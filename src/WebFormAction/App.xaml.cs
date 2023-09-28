using CefSharp;
using CefSharp.Wpf;
using Prism.Ioc;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using WebFormAction.Views;

namespace WebFormAction
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        public static AppData Data { get; set; } = new AppData();

        public App()
        {
            //Add Custom assembly resolver
            AppDomain.CurrentDomain.AssemblyResolve += Resolver;

            //Any CefSharp references have to be in another method with NonInlining
            // attribute so the assembly rolver has time to do it's thing.
            InitializeCefSharp();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializeCefSharp()
        {
            var settings = new CefSettings();

            // Set BrowserSubProcessPath based on app bitness at runtime
            settings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                   Environment.Is64BitProcess ? "x64" : "x86",
                                                   "CefSharp.BrowserSubprocess.exe");
            settings.Locale = "zh-CHS";
            settings.AcceptLanguageList = "zh-CN,zh";
            settings.LogSeverity = LogSeverity.Disable;
            settings.MultiThreadedMessageLoop = true;
            settings.CachePath = Directory.GetCurrentDirectory() + @"\Cache";

            // Make sure you set performDependencyCheck false
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);

            CefSharpSettings.ShutdownOnExit = true;
            CefSharpSettings.WcfEnabled = true;
        }

        // Will attempt to load missing assembly from either x86 or x64 subdir
        // Required by CefSharp to load the unmanaged dependencies when running using AnyCPU
        private static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                       Environment.Is64BitProcess ? "x64" : "x86",
                                                       assemblyName);

                return File.Exists(archSpecificPath)
                           ? Assembly.LoadFile(archSpecificPath)
                           : null;
            }

            return null;
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}