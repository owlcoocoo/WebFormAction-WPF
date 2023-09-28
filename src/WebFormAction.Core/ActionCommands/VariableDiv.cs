using System;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class VariableDiv : ActionCommand
    {
        public VariableDiv()
        {
            Id = "09ae1184-b0c4-4f63-a927-6da1cac7cdb1";
            Name = "变量相除";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "值", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string varName = Parameters[0].Value;
            string varValue = context.GetVariableValue(varName);
            string value = Parameters[1].Value;
            value = (Convert.ToDecimal(varValue) / Convert.ToDecimal(value)).ToString();
            context.SetVariableValue(varName, value, Name);
        }
    }
}
