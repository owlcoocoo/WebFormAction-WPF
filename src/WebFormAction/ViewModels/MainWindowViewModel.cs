using Microsoft.Win32;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WebFormAction.Core;
using WebFormAction.Core.Interfaces;
using WebFormAction.Core.Models;
using WebFormAction.Views;

namespace WebFormAction.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            ActionCommands = App.Data.ActionCommands;
        }

        private string _title = App.Data.Title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string FilePath { get; set; }

        private ActionExecuter actionExecuter;

        public ObservableCollection<ActionViewModel> _actionCommands;
        public ObservableCollection<ActionViewModel> ActionCommands
        {
            get { return _actionCommands; }
            set { SetProperty(ref _actionCommands, value); }
        }

        public ActionViewModel _currentActionCommand;
        public ActionViewModel CurrentActionCommand
        {
            get { return _currentActionCommand; }
            set { SetProperty(ref _currentActionCommand, value); }
        }

        private bool isOpenedVariableWindow = false;
        public DelegateCommand ButtonVariableClick => new DelegateCommand(() =>
        {
            if (!isOpenedVariableWindow)
            {
                VariableWindow window = new VariableWindow();
                window.Owner = App.Current.MainWindow;
                window.Show();
                window.Closed += new EventHandler((sender, e) =>
                {
                    (sender as VariableWindow).dataGrid.CancelEdit(DataGridEditingUnit.Row);
                    isOpenedVariableWindow = false;
                });

                isOpenedVariableWindow = true;
            }
        });

        private bool isOpenedActionWindow = false;
        public DelegateCommand ButtonActionClick => new DelegateCommand(() =>
        {
            if (!isOpenedActionWindow)
            {
                ActionWindow window = new ActionWindow();
                window.Owner = App.Current.MainWindow;
                window.Show();
                window.Closed += new EventHandler((sender, e) =>
                {
                    isOpenedActionWindow = false;
                });

                isOpenedActionWindow = true;
            }
        });

        public DelegateCommand ButtonAboutClick => new DelegateCommand(() =>
        {
            MessageBox.Show($"软件版本：{App.Data.Version}", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
        });

        public DelegateCommand Edit => new DelegateCommand(() =>
        {
            if (CurrentActionCommand == null)
                return;

            if (!isOpenedActionWindow)
            {
                ActionWindow window = new ActionWindow(CurrentActionCommand);
                window.Owner = App.Current.MainWindow;
                window.Show();
                window.Closed += new EventHandler((sender, e) =>
                {
                    isOpenedActionWindow = false;
                });

                isOpenedActionWindow = true;
            }
        });

        public DelegateCommand Delete => new DelegateCommand(() =>
        {
            if (CurrentActionCommand == null)
                return;

            ActionCommands.Remove(CurrentActionCommand);
        });

        private bool isOpenedWebBrowserWindow = false;
        public DelegateCommand<Window> OpenWebBrowser => new DelegateCommand<Window>((window) =>
        {
            if (!isOpenedWebBrowserWindow)
            {
                isOpenedWebBrowserWindow = true;
            }
        });

        private void NewFile()
        {
            FilePath = null;
            Title = App.Data.Title;
            ActionCommands.Clear();
        }

        public DelegateCommand New => new DelegateCommand(() =>
        {
            NewFile();
        });


        public DelegateCommand Open => new DelegateCommand(() =>
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = App.Data.FileFilter;
            if (fileDialog.ShowDialog() == true)
            {
                NewFile();
                FilePath = fileDialog.FileName;
            }
            else
                return;

            var stream = new StreamReader(FilePath, false);

            try
            {
                var data = JsonConvert.DeserializeObject<AppData>(stream.ReadToEnd());

                App.Data.ActionCommands = data.ActionCommands;
                App.Data.Variables = data.Variables;

                ActionCommands = App.Data.ActionCommands;
            }
            catch
            {
                MessageBox.Show("读取失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            stream.Close();

            Title = App.Data.Title + " - " + FilePath;
        });

        private string SaveFile(string filePath)
        {
            if (filePath == null)
            {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = App.Data.FileFilter;
                if (fileDialog.ShowDialog() == true)
                {
                    filePath = fileDialog.FileName;
                }
                else
                    return null;
            }

            var stream = new StreamWriter(filePath, false);
            try
            {
                var json = JsonConvert.SerializeObject(App.Data);
                stream.Write(json);
                stream.Close();
            }
            catch
            {
                MessageBox.Show("保存失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            stream.Close();

            if (filePath != null)
                Title = App.Data.Title + " - " + filePath;

            return filePath;
        }

        public DelegateCommand Save => new DelegateCommand(() =>
        {
            FilePath = SaveFile(FilePath);
        });

        public bool _isStarted;
        public bool IsStarted
        {
            get { return _isStarted; }
            set { SetProperty(ref _isStarted, value); }
        }

        public bool _isCanPause;
        public bool IsCanPause
        {
            get { return _isCanPause; }
            set { SetProperty(ref _isCanPause, value); }
        }

        public bool _isCanPlay = true;
        public bool IsCanPlay
        {
            get { return _isCanPlay; }
            set { SetProperty(ref _isCanPlay, value); }
        }

        public string PlayTip
        {
            get
            {
                if (IsStarted)
                    return "继续";
                return "执行";
            }
        }

        private string _statusText;
        public string StatusText
        {
            get { return _statusText; }
            set { SetProperty(ref _statusText, value); }
        }

        private int? _progress;
        public int? Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }

        public DelegateCommand ExecuteSelected => new DelegateCommand(() =>
        {
            if (CurrentActionCommand == null)
                return;

            List<ActionCommandModel> cmdList = new List<ActionCommandModel>();
            cmdList.Add(CurrentActionCommand.ActionCommand);

            var invokeDispatcher = new ActionExecuter.InvokeDispatcher((a) =>
            {
                var dispatcher = App.Data.WebBrowser.Dispatcher;
                if (dispatcher.HasShutdownStarted)
                    return null;
                return dispatcher.Invoke(a);
            });

            var executer = new ActionExecuter(App.Data.WebBrowser, App.Data.Variables.ToList<IActionVariable>(), invokeDispatcher);
            executer.ActionExceptionEvent += (ex) =>
            {
                StatusText = ex.Message;
                WriteErrorLogFile(ex);
            };
            executer.Execute(cmdList);
        });

        public DelegateCommand Start => new DelegateCommand(() =>
        {
            var cmdList = App.Data.GetActionCommandList();
            if (cmdList.Count < 1)
            {
                return;
            }

            Progress = 35;
            StatusText = "正在执行...";

            if (IsStarted)
            {
                IsCanPlay = false;
                IsCanPause = true;

                actionExecuter.Continue();

                return;
            }

            var invokeDispatcher = new ActionExecuter.InvokeDispatcher((a) =>
            {
                var dispatcher = App.Data.WebBrowser.Dispatcher;
                if (dispatcher.HasShutdownStarted)
                    return null;
                return dispatcher.Invoke(a);
            });

            actionExecuter = new ActionExecuter(App.Data.WebBrowser, App.Data.Variables.ToList<IActionVariable>(), invokeDispatcher);
            actionExecuter.ActionEndEvent += ActionExecuter_ActionEndEvent;
            actionExecuter.BeforeExecuteCommandEvent += ActionExecuter_BeforeExecuteCommandEvent;
            actionExecuter.AfterExecuteCommandEvent += ActionExecuter_AfterExecuteCommandEvent;
            actionExecuter.ActionExceptionEvent += ActionExecuter_ActionExceptionEvent;
            actionExecuter.Execute(cmdList);

            IsStarted = true;
            IsCanPlay = false;
            IsCanPause = true;
        });

        private void WriteErrorLogFile(Exception ex)
        {
            string time = DateTime.Now.ToString("yyyyMMdd");
            Directory.CreateDirectory("Logs");
            File.AppendAllText($"Logs/{ time }.log", $"******{DateTime.Now}******\r\n{ex.ToString()}\r\n\r\n");
        }

        private void ActionExecuter_ActionExceptionEvent(Exception ex)
        {
            Stop.Execute();
            StatusText = ex.Message;
            WriteErrorLogFile(ex);
        }

        private void ActionExecuter_BeforeExecuteCommandEvent(ActionContext context, ActionCommand cmd)
        {
            var curCmd = ActionCommands[context.CurrentLocation];
            curCmd.Visibility = Visibility.Visible;
        }

        private void ActionExecuter_AfterExecuteCommandEvent(ActionContext context, ActionCommand cmd)
        {
            var curCmd = ActionCommands[context.CurrentLocation];
            curCmd.Visibility = Visibility.Collapsed;
        }

        private void ActionExecuter_ActionEndEvent()
        {
            IsStarted = false;
            IsCanPlay = true;
            IsCanPause = false;

            RaisePropertyChanged("PlayTip");

            Progress = 100;
            StatusText = "执行完毕";
        }

        public DelegateCommand Pause => new DelegateCommand(() =>
        {
            actionExecuter.Pause();

            IsCanPause = false;
            IsCanPlay = true;

            RaisePropertyChanged("PlayTip");
            StatusText = "暂停中";
        });

        public DelegateCommand Stop => new DelegateCommand(() =>
        {
            actionExecuter.Stop();

            IsStarted = false;
            IsCanPlay = true;
            IsCanPause = false;

            RaisePropertyChanged("PlayTip");

            Progress = null;
            StatusText = "已停止";
        });
    }
}
