using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class EnableJsDialog : ActionCommand
    {
        public EnableJsDialog()
        {
            Id = "e746c5a4-1d9a-42ad-830f-b12e41bd8e7d";
            Name = "允许网页弹对话框";
            Type = ActionCommandType.Sys;
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            context.WebBrowser.JsDialogHandler = null;
        }
    }
}
