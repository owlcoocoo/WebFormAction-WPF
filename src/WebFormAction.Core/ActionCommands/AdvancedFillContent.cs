using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class AdvancedFillContent : ActionCommand
    {
        public AdvancedFillContent()
        {
            Id = "a571df51-70ec-40a6-9932-6ec97c936b78";
            Name = "高级填写";
            Type = ActionCommandType.Web;
            Parameters.Add(new ActionParameterModel(1, "元素", ActionParameterType.Element));
            Parameters.Add(new ActionParameterModel(2, "内容", ActionParameterType.Text));
        }

        public override void Execute(ActionContext context)
        {
            var command = new AdvancedClick();
            command.Parameters[0].Value = Parameters[0].Value;
            command.Execute(context);

            //(context.WebBrowser as ChromiumWebBrowser).Invoke(new Action(() =>
            //{
            //    Clipboard.SetText(Parameters[1].Value.ToString());
            //    var host = context.WebBrowser.GetBrowserHost();
            //    host.SetFocus(true);
            //    context.WebBrowser.SelectAll();
            //    context.WebBrowser.Paste();
            //}
            //));
        }
    }
}
