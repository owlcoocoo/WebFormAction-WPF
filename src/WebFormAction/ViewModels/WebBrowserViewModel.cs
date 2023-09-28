using CefSharp;
using CefSharp.Wpf;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using WebFormAction.Events;
using WebFormAction.Handlers;

namespace WebFormAction.ViewModels
{
    public class WebBrowserViewModel : BindableBase
    {
        IEventAggregator _ea;

        public WebBrowserViewModel(IEventAggregator ea)
        {
            PropertyChanged += OnPropertyChanged;

            _ea = ea;
            _ea.GetEvent<WebBrowserWindowEvent>().Subscribe(MessageReceived);
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Address":
                    break;

                case "WebBrowser":
                    if (WebBrowser != null)
                    {
                        App.Data.WebBrowser = WebBrowser;

                        webBrowser.JavascriptObjectRepository.Register("MouseEventData", App.Data.MouseEventData, false);

                        WebBrowser.LifeSpanHandler = new LifeSpanHandler();

                        HwndSource source = PresentationSource.FromVisual(WebBrowser) as HwndSource;
                        if (source != null) source.AddHook(WndProc);
                    }
                    break;
            }
        }

        private string address;
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        private string addressEditable;
        public string AddressEditable
        {
            get { return addressEditable; }
            set { SetProperty(ref addressEditable, value); }
        }

        private ChromiumWebBrowser webBrowser;
        public ChromiumWebBrowser WebBrowser
        {
            get { return webBrowser; }
            set { SetProperty(ref webBrowser, value); }
        }

        private void MessageReceived(string message)
        {
            if (message == "GetElement")
            {
                StartGetElement();
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_KEYUP = 0x0101;
            if (msg == WM_KEYUP)
            {
                if ((int)wParam == 0x7b)
                {
                    var browser = WebBrowser.GetBrowser().GetHost();
                    browser.ShowDevTools();
                }
            }

            if (IsGettingElement)
            {
                const int WM_LBUTTONDOWN = 0x0201;
                const int WM_RBUTTONDOWN = 0x0204;


                if (msg == WM_LBUTTONDOWN)
                {
                    handled = true;
                    App.Data.MouseEventData.IsMouseLeftDown = true;
                }
                else if (msg == WM_RBUTTONDOWN)
                {
                    handled = true;
                    App.Data.MouseEventData.IsMouseRightDown = true;
                }
            }

            return IntPtr.Zero;
        }


        public bool IsGettingElement { get; set; }

        private Task task;


        private void Task_Elapsed()
        {
            WindowInfo windowInfo = new WindowInfo();
            WebBrowser.Dispatcher.Invoke(() =>
            {
                var window = Window.GetWindow(WebBrowser);
                windowInfo.SetAsChild(new WindowInteropHelper(window).Handle, 0, 0, 0, 0);
            });

            while (IsGettingElement)
            {
                if (!App.Data.MouseEventData.IsRightMenu)
                {
                    var browser = WebBrowser.GetBrowser().GetHost();

                    Point point = WebBrowser.Dispatcher.Invoke(() =>
                    {
                        return Mouse.GetPosition(WebBrowser);
                    });

                    browser.ShowDevTools(windowInfo, (int)point.X, (int)point.Y);
                }

                if (App.Data.MouseEventData.IsMouseLeftDown)
                {
                    // 获取元素标识
                    string js = Core.Utilities.Scripts.ReadResource("DOMPresentationUtils.js");
                    List<long> frameIdentifiers = App.Data.WebBrowser.GetMainFrame().Browser.GetFrameIdentifiers();
                    if (frameIdentifiers.Count > 0)
                    {
                        string js2 = js + "var element = document.elementFromPoint(MouseEventData.X, MouseEventData.Y);return DOMPresentationUtils.cssPath(element, true);";
                        js2 = "(function (){" + js2 + "})();";
                        var frame = App.Data.WebBrowser.GetBrowser().GetFrame(frameIdentifiers[App.Data.MouseEventData.FrameIndex]);
                        var t = frame?.EvaluateScriptAsync(js2, "");
                        t.Wait(3000);
                        App.Data.MouseEventData.ElementSign = $"{App.Data.MouseEventData.FrameIndex}|{t.Result?.Result?.ToString()}";
                        App.Data.MouseEventData.ElementSign = App.Data.MouseEventData.ElementSign.Replace("\\", "\\\\");
                    }

                    StopGetElement();

                    //if (IsAnalyse)
                    //{
                    //    js2 = js + Core.Utilities.Scripts.ReadResource("AnalyseElement.js");
                    //    t = frame?.EvaluateScriptAsync(js2, "");
                    //    t.Wait(3000);
                    //    MouseEventData.ElementSelectorData = (List<object>)t.Result?.Result;

                    //    ShowSelector();
                    //}
                }
                else if (App.Data.MouseEventData.IsMouseRightDown)
                {
                    StopGetElement();
                    App.Data.MouseEventData.IsMouseRightDown = true;
                }

                Task.Delay(350).Wait();
            }
        }

        private void InitMouseEvent()
        {
            string js = @"
                        (async function(){
                            await CefSharp.BindObjectAsync('MouseEventData');
                            if (this.frameElement) {
                                var style = this.frameElement.style['display'];
                                if (style && style.toLowerCase() == 'none') {
                                    var array = MouseEventData.InvalidFrameIdentifiers;
                                    if (!array) {
                                        array = [];
                                    }
                                    array.push(nIdentifier);
                                    MouseEventData.InvalidFrameIdentifiers = array;
                                    return;
                                }
                            } 
                            if (!window.mouseEventFunc) {
                                window.mouseEventFunc = function() {
                                    if(MouseEventData.IsMouseLeftDown) {
                                        return;
                                    }
                                    MouseEventData.X=event.clientX;
                                    MouseEventData.Y=event.clientY;
                                    MouseEventData.FrameIndex=nIdentifier;
                                };
                            }
                            document.onmousemove=window.mouseEventFunc;
                        })();
                        ";

            List<long> frameIdentifiers = App.Data.WebBrowser.GetBrowser().GetFrameIdentifiers();
            App.Data.MouseEventData.InvalidFrameIdentifiers = null;
            for (int i = 0; i < frameIdentifiers.Count; i++)
            {
                var frame = App.Data.WebBrowser.GetBrowser().GetFrame(frameIdentifiers[i]);
                string js2 = "var nIdentifier=" + i.ToString() + ";" + js;
                frame?.ExecuteJavaScriptAsync(js2, "");
            }
        }

        public void StartGetElement()
        {
            IsGettingElement = true;

            App.Data.MouseEventData.IsMouseLeftDown = false;
            App.Data.MouseEventData.IsMouseRightDown = false;
            App.Data.MouseEventData.ElementSign = null;
            App.Data.MouseEventData.FrameIndex = 0;

            InitMouseEvent();

            if (!App.Data.MouseEventData.IsRightMenu)
            {
                task = new Task(Task_Elapsed);
                task.Start();
            }
        }

        public void StopGetElement()
        {
            App.Data.MouseEventData.IsMouseLeftDown = false;
            App.Data.MouseEventData.IsMouseRightDown = false;
            App.Data.MouseEventData.IsRightMenu = false;

            IsGettingElement = false;

            //WebBrowser.CloseDevTools();

            _ea.GetEvent<WebBrowserWindowEvent>().Publish("EndGetElement");
        }
    }
}
