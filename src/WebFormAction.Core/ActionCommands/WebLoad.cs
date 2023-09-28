using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class WebLoad : ActionCommand
    {
        public WebLoad()
        {
            Id = "2845521a-9a45-40b0-a3a7-124b5be365b5";
            Name = "访问网址";
            Type = ActionCommandType.Web;
            Parameters.Add(new ActionParameterModel(1, "网址", ActionParameterType.Text));
        }

        public override void Execute(ActionContext context)
        {
            context.WebBrowser.Load(Parameters[0].Value);
        }
    }
}
