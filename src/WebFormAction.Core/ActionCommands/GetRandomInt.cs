using System;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class GetRandomInt : ActionCommand
    {
        public GetRandomInt()
        {
            Id = "f1dea3b8-a043-465c-8817-5bd0dc7c2f1b";
            Name = "取随机整数";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "存放数值的变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "最小整数", ActionParameterType.Text));
            Parameters.Add(new ActionParameterModel(3, "最大整数", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string str = Parameters[1].Value;
            string str2 = Parameters[2].Value;

            Random ran = new Random();
            int n = ran.Next(Convert.ToInt32(str), Convert.ToInt32(str2) + 1);

            str = Parameters[0].Value;
            context.SetVariableValue(str, n.ToString(), Name);
        }
    }
}
