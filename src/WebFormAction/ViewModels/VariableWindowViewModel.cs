using Prism.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace WebFormAction.ViewModels
{
    public class VariableWindowViewModel : ViewModelBase, IDataErrorInfo
    {
        public VariableWindowViewModel()
        {
        }

        public Window Window { get; set; }

        public ObservableCollection<VariableViewModel> VariableModels { get; set; } = App.Data.Variables;

        public string Name { get; set; }

        public DelegateCommand<string> AddVariable => new DelegateCommand<string>((name) =>
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ErrorsContainer.SetErrors("Name", new string[] { "请输入变量名。" });
                return;
            }

            if (VariableModels.Count(a => a.Name == name) > 0)
            {
                ErrorsContainer.SetErrors("Name", new string[] { "此变量名已存在，请输入其它的变量名。" });
                return;
            }

            VariableModels.Add(new VariableViewModel(name) { DataList = VariableModels });
        });

        public DelegateCommand<VariableViewModel> DeleteOne => new DelegateCommand<VariableViewModel>((item) =>
        {
            VariableModels.Remove(item);
        });

        public Visibility _selectButtonVisibility = Visibility.Collapsed;
        public Visibility SelectButtonVisibility
        {
            get { return _selectButtonVisibility; }
            set { SetProperty(ref _selectButtonVisibility, value); }
        }

        public VariableViewModel SelectedVariable { get; set; }

        public DelegateCommand<VariableViewModel> ConfirmSelect => new DelegateCommand<VariableViewModel>((item) =>
        {
            SelectedVariable = item;
            Window.DialogResult = true;
        });

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                ErrorsContainer.ClearErrors();

                string result = null;

                if (columnName == "Name")
                {
                    if (VariableModels.Count(a => a.Name == Name) > 0)
                    {
                        result = "此变量名已存在，请输入其它的变量名。";
                    }
                }

                return result;
            }
        }

    }
}
