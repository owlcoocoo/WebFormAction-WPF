using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class SelectListBoxItem : ActionCommand
    {
        public SelectListBoxItem()
        {
            Id = "bf765235-85de-48a4-b967-7e0f9892dc6e";
            Name = "列表项选择";
            Type = ActionCommandType.Web;
            Parameters.Add(new ActionParameterModel(1, "元素或父元素", ActionParameterType.Element));
            Parameters.Add(new ActionParameterModel(2, "索引", ActionParameterType.Text));
        }

        public override void Execute(ActionContext context)
        {
            string script = Utilities.Scripts.ReadResource("SelectListBoxItem.js");
            context.RunScriptAndNoResult(script, Parameters);
        }
    }
}
