using Prism.Mvvm;
using System;
using System.Collections;
using System.ComponentModel;

namespace WebFormAction.ViewModels
{
    public class ViewModelBase : BindableBase, INotifyDataErrorInfo
    {
        private ErrorsContainer<string> _errorsContainer;
        public ErrorsContainer<string> ErrorsContainer
        {
            get
            {
                if (_errorsContainer == null)
                {
                    _errorsContainer = new ErrorsContainer<string>((propName) =>
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propName));
                    });
                }

                return _errorsContainer;
            }
            set { _errorsContainer = value; }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool HasErrors => ErrorsContainer.HasErrors;
        public IEnumerable GetErrors(string propertyName) => ErrorsContainer.GetErrors(propertyName);
    }
}
