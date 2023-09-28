using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class SetVariableValue : ActionCommand
    {
        public SetVariableValue()
        {
            Id = "6aeda780-f1cc-450d-9cfb-6d7aa246083e";
            Name = "设置变量值";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "值", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string varName = Parameters[0].Value.ToString();
            string varValue = Parameters[1].Value.ToString();
            context.SetVariableValue(varName, varValue, Name);
        }
    }
}
