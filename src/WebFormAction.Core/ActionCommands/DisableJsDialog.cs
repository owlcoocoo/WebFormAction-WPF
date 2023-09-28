using WebFormAction.Core.Handlers;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class DisableJsDialog : ActionCommand
    {
        public DisableJsDialog()
        {
            Id = "a9f93086-00f7-4411-8819-550c8711cb6b";
            Name = "禁止网页弹对话框";
            Type = ActionCommandType.Sys;
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            JsDialogHandler jsDialogHandler = new JsDialogHandler();
            context.WebBrowser.JsDialogHandler = jsDialogHandler;
        }
    }
}
