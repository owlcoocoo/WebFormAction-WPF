using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WebFormAction.ViewModels;

namespace WebFormAction.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _context;

        public MainWindow()
        {
            InitializeComponent();

            _context = DataContext as MainWindowViewModel;
        }

        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = sender as TreeViewItem;
            if (treeViewItem != null)
            {
                if (treeViewItem.DataContext is ActionViewModel)
                    _context.CurrentActionCommand = treeViewItem.DataContext as ActionViewModel;
            }
        }

        private void CalcSize()
        {
            treeRow.MaxHeight = 160;
            wbRow.MaxHeight = this.ActualHeight - 56 - 36 * 2 - 13 - treeRow.MaxHeight;
        }

        private void GridSplitter_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            CalcSize();
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CalcSize();
        }
    }
}
