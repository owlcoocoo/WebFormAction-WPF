using System;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class GetSplitStringCount : ActionCommand
    {
        public GetSplitStringCount()
        {
            Id = "4734e163-60aa-41c4-8fe9-0fae2b2a63b2";
            Name = "取分隔文本数量";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "文本变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "存放分隔数量的变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(3, "分隔符", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string varName = Parameters[0].Value;
            string varValue = context.GetVariableValue(varName);
            string str = Parameters[2].Value;
            string[] strArray = varValue.Split(str.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string varStoreName = Parameters[1].Value;

            context.SetVariableValue(varStoreName, strArray.Length.ToString(), Name);
        }
    }
}
