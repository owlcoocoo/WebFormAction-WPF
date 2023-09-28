using System.Windows;
using WebFormAction.ViewModels;

namespace WebFormAction.Views
{
    /// <summary>
    /// VariableWindow.xaml 的交互逻辑
    /// </summary>
    public partial class VariableWindow : Window
    {
        private VariableWindowViewModel _context;

        public VariableWindow()
        {
            InitializeComponent();

            _context = DataContext as VariableWindowViewModel;
            _context.Window = this;
        }

        public VariableWindow(bool selectable) : this()
        {
            _context.SelectButtonVisibility = selectable ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
