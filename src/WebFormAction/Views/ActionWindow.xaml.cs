using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WebFormAction.Core.Models;
using WebFormAction.ViewModels;

namespace WebFormAction.Views
{
    /// <summary>
    /// ActionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ActionWindow : Window
    {
        private readonly ActionWindowViewModel _dataContext;

        public ActionWindow()
        {
            InitializeComponent();
            _dataContext = DataContext as ActionWindowViewModel;

            RaiseMouseWheelEvent(listBoxAllCmd);
            RaiseMouseWheelEvent(listBoxWebCmd);
            RaiseMouseWheelEvent(listBoxSysCmd);
        }

        public ActionWindow(ActionViewModel model) : this()
        {
            if (model != null)
            {
                _dataContext.IsModify = true;
                _dataContext.CurrentActionCommand = model;
                _dataContext.ModifyActionCommand = model;
                listBoxParam.SelectedIndex = 0;
            }
        }

        private void RaiseMouseWheelEvent(Selector selector)
        {
            selector.PreviewMouseWheel += (sender, e) =>
            {
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                selector.RaiseEvent(eventArg);
            };
        }

        private void Reset()
        {
            textbox.Text = "";
            _dataContext.IsEnableEdit = false;
        }

        private void listBoxSysCmd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Reset();

            _dataContext.CurrentActionCommand = listBoxSysCmd.SelectedItem as ActionViewModel;
            listBoxParam.SelectedIndex = 0;
        }

        private void listBoxWebCmd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Reset();

            _dataContext.CurrentActionCommand = listBoxWebCmd.SelectedItem as ActionViewModel;
            listBoxParam.SelectedIndex = 0;
        }

        private void listBoxAllCmd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Reset();

            _dataContext.CurrentActionCommand = listBoxAllCmd.SelectedItem as ActionViewModel;
            listBoxParam.SelectedIndex = 0;
        }

        private void listBoxParam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _dataContext.CurrentActionParameter = listBoxParam.SelectedItem as ActionParameterModel;
            if (_dataContext.CurrentActionParameter != null)
            {
                _dataContext.IsEnableEdit = true;
                textbox.Text = _dataContext.CurrentActionParameter.Value;
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
