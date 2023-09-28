using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows;
using WebFormAction.Core.Models;

namespace WebFormAction.ViewModels
{
    public class ActionViewModel : BindableBase
    {
        public ActionCommandModel ActionCommand { get; set; }

        public ObservableCollection<ActionParameterModel> Parameters { get; set; }


        private Visibility _visibility = Visibility.Collapsed;
        public Visibility Visibility
        {
            get { return _visibility; }
            set { SetProperty(ref _visibility, value); }
        }
    }
}
