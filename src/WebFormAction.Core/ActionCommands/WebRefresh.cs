using CefSharp;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class WebRefresh : ActionCommand
    {
        public WebRefresh()
        {
            Id = "8e1bf227-b1c4-4364-8546-40c5b34df685";
            Name = "刷新页面";
            Type = ActionCommandType.Web;
        }

        public override void Execute(ActionContext context)
        {
            context.WebBrowser.Reload();
        }
    }
}
