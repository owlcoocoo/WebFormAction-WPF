using System;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class VariableMul : ActionCommand
    {
        public VariableMul()
        {
            Id = "4b7deec0-830a-44db-8e06-04002822c5f6";
            Name = "变量相乘";
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
            value = (Convert.ToDecimal(varValue) * Convert.ToDecimal(value)).ToString();
            context.SetVariableValue(varName, value, Name);
        }
    }
}
