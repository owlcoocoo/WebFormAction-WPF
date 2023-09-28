using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class Click : ActionCommand
    {
        public Click()
        {
            Id = "48d0443b-7820-412f-a83f-d2e746195084";
            Name = "点击";
            Type = ActionCommandType.Web;
            Parameters.Add(new ActionParameterModel(1, "元素", ActionParameterType.Element));
        }

        public override void Execute(ActionContext context)
        {
            string script = Utilities.Scripts.ReadResource("Click.js");
            context.RunScriptAndNoResult(script, Parameters);
        }
    }
}
