using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WebFormAction.Core;
using WebFormAction.Core.Models;
using WebFormAction.Events;
using WebFormAction.Views;

namespace WebFormAction.ViewModels
{
    public class ActionWindowViewModel : BindableBase
    {
        IEventAggregator _ea;

        public ObservableCollection<ActionViewModel> AllActionCommands { get; set; } = new ObservableCollection<ActionViewModel>();
        public ObservableCollection<ActionViewModel> WebActionCommands { get; set; } = new ObservableCollection<ActionViewModel>();
        public ObservableCollection<ActionViewModel> SysActionCommands { get; set; } = new ObservableCollection<ActionViewModel>();

        private ActionViewModel _currentActionCommand;
        public ActionViewModel CurrentActionCommand
        {
            get { return _currentActionCommand; }
            set { SetProperty(ref _currentActionCommand, value); }
        }

        public ActionViewModel ModifyActionCommand { get; set; }

        private ActionParameterModel _currentActionParameter;
        public ActionParameterModel CurrentActionParameter
        {
            get { return _currentActionParameter; }
            set { SetProperty(ref _currentActionParameter, value); }
        }

        private bool _isEnableEdit;
        public bool IsEnableEdit
        {
            get { return _isEnableEdit; }
            set { SetProperty(ref _isEnableEdit, value); }
        }

        public bool IsModify { get; set; }

        private bool _isEnableGetElement = true;
        public bool IsEnableGetElement
        {
            get { return _isEnableGetElement; }
            set { SetProperty(ref _isEnableGetElement, value); }
        }

        public ActionWindowViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _ea.GetEvent<WebBrowserWindowEvent>().Subscribe(MessageReceived);

            var list = new ActionContext(null, null).CommandList;
            foreach (var item in list)
            {
                var cmd = new ActionViewModel();
                cmd.ActionCommand = item;
                cmd.Parameters = new ObservableCollection<ActionParameterModel>(item.Parameters);
                AllActionCommands.Add(cmd);

                if (cmd.ActionCommand.Type == ActionCommandType.Web)
                    WebActionCommands.Add(cmd);
                else if (cmd.ActionCommand.Type == ActionCommandType.Sys)
                    SysActionCommands.Add(cmd);
            }
        }

        public DelegateCommand<string> ButtonVariableClick => new DelegateCommand<string>((isSelectVariable) =>
        {
            VariableWindow window = new VariableWindow(true);
            window.Owner = App.Current.MainWindow;
            if (window.ShowDialog() == true)
            {
                if (CurrentActionParameter == null)
                    return;

                var context = window.DataContext as VariableWindowViewModel;
                if (CurrentActionParameter.Type == ActionParameterType.Variable)
                    CurrentActionParameter.Value = $"$({context.SelectedVariable.Name})$";
                else
                    CurrentActionParameter.Value += $"$({context.SelectedVariable.Name})$";
                RaisePropertyChanged("CurrentActionParameter");
            }
        });

        public DelegateCommand<Window> GetElement => new DelegateCommand<Window>((window) =>
        {
            IsEnableGetElement = false;
            _ea.GetEvent<WebBrowserWindowEvent>().Publish("GetElement");
        });

        private void MessageReceived(string message)
        {
            if (message == "EndGetElement")
            {
                IsEnableGetElement = true;
                if (CurrentActionParameter != null)
                {
                    CurrentActionParameter.Value = App.Data.MouseEventData.ElementSign;
                    RaisePropertyChanged("CurrentActionParameter");
                }
            }
        }

        public DelegateCommand<Window> ButtonConfirmClick => new DelegateCommand<Window>((window) =>
        {
            if (CurrentActionCommand == null)
                return;

            if (!IsModify)
            {
                App.Data.ActionCommands.Add(CurrentActionCommand);
            }
            else
            {
                int index = Array.IndexOf(App.Data.ActionCommands.ToArray(), ModifyActionCommand);
                App.Data.ActionCommands.RemoveAt(index);
                App.Data.ActionCommands.Insert(index, CurrentActionCommand);
            }

            window?.Close();
        });

    }
}
