using System.Collections.ObjectModel;
using System.Linq;
using WebFormAction.Core.Interfaces;

namespace WebFormAction.ViewModels
{
    public class VariableViewModel : ViewModelBase, IActionVariable
    {
        public VariableViewModel(string name)
        {
            Name = name;
        }

        public ObservableCollection<VariableViewModel> DataList { get; set; }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (DataList != null)
                {
                    ErrorsContainer.ClearErrors();
                    if (DataList.Count(a => a.Name == value) > 0)
                    {
                        ErrorsContainer.SetErrors("Name", new string[] { $"变量名“{value}”已存在。" });
                        //return;
                    }
                }

                _name = value;
            }

        }

        public dynamic Value { get; set; }
    }
}
