using System;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class Delay : ActionCommand
    {
        public Delay()
        {
            Id = "07284fad-d858-4f7e-80ec-c40e50b861c0";
            Name = "延时";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "最小时间-秒", ActionParameterType.Text));
            Parameters.Add(new ActionParameterModel(2, "最大时间-秒", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            decimal min = Convert.ToDecimal(Parameters[0].Value);
            decimal max = Convert.ToDecimal(Parameters[1].Value);
            context.Delay(min, max);
        }
    }
}
