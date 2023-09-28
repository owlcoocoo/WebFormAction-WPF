using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class FillContent : ActionCommand
    {
        public FillContent()
        {
            Id = "679f8a96-089c-4e7d-a4e3-2b4b51456074";
            Name = "填写内容";
            Type = ActionCommandType.Web;
            Parameters.Add(new ActionParameterModel(1, "元素", ActionParameterType.Element));
            Parameters.Add(new ActionParameterModel(2, "内容", ActionParameterType.Text));
        }

        public override void Execute(ActionContext context)
        {
            string script = Utilities.Scripts.ReadResource("FillContent.js");
            context.RunScriptAndNoResult(script, Parameters);
        }
    }
}
