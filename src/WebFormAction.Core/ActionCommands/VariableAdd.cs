using System;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class VariableAdd : ActionCommand
    {
        public VariableAdd()
        {
            Id = "b6351d4c-a816-45f4-b9b2-a62847d08b2b";
            Name = "变量相加";
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
            value = (Convert.ToDecimal(varValue) + Convert.ToDecimal(value)).ToString();
            context.SetVariableValue(varName, value, Name);
        }
    }
}
