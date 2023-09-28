using System;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class GetASectionStringBySplit : ActionCommand
    {
        public GetASectionStringBySplit()
        {
            Id = "85c168be-e79d-4fbd-b2f2-f6fb12a64c62";
            Name = "取指定一段文本-按分隔符";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "文本变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "存放内容的变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(3, "分隔符", ActionParameterType.Text));
            Parameters.Add(new ActionParameterModel(4, "第几段", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string varName = Parameters[0].Value;
            string varValue = context.GetVariableValue(varName);
            string str = Parameters[2].Value;
            string[] strArray = str.Split(varValue.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            str = Parameters[3].Value;
            if (str.Trim() == "")
                str = "1";
            int n = Convert.ToInt32(str);
            n -= 1;
            varName = Parameters[1].Value;

            if (n < strArray.Length)
                context.SetVariableValue(varName, strArray[n], Name);
        }
    }
}
