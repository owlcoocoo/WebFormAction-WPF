using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class RandomClick : ActionCommand
    {
        public RandomClick()
        {
            Id = "1c4cead5-06cc-4c6c-b48b-86f7498f44f7";
            Name = "随机点击";
            Type = ActionCommandType.Web;
            Parameters.Add(new ActionParameterModel(1, "父元素", ActionParameterType.Element));
        }

        public override void Execute(ActionContext context)
        {
            string script = Utilities.Scripts.ReadResource("RandomClick.js");
            context.RunScriptAndNoResult(script, Parameters);
        }
    }
}
