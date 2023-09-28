using CefSharp.Wpf;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebFormAction.Core.Models;
using WebFormAction.Models;
using WebFormAction.ViewModels;

namespace WebFormAction
{
    public class AppData
    {
        [JsonIgnore]
        public string Title { get; set; } = "网页表单助手";

        [JsonIgnore]
        public string Version { get; set; } = "v2.0-2021072901";

        /// <summary>
        /// Web Action Command List
        /// </summary>
        [JsonIgnore]
        public string FileFilter { get; set; } = "*.wacl|*.wacl";

        [JsonIgnore]
        public ChromiumWebBrowser WebBrowser { get; set; }

        [JsonIgnore]
        public MouseEventModel MouseEventData { get; set; } = new MouseEventModel();

        public ObservableCollection<VariableViewModel> Variables { get; set; } = new ObservableCollection<VariableViewModel>();
        public ObservableCollection<ActionViewModel> ActionCommands { get; set; } = new ObservableCollection<ActionViewModel>();

        public List<ActionCommandModel> GetActionCommandList()
        {
            var list = new List<ActionCommandModel>();
            foreach (var item in ActionCommands)
            {
                list.Add(item.ActionCommand);
            }
            return list;
        }
    }
}
