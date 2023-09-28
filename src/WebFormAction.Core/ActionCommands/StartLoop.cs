using System;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class StartLoop : ActionCommand
    {
        public StartLoop()
        {
            Id = "4c0d83ac-bc1f-4f5f-87be-1c242ce1ee24";
            Name = "开始循环";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "额外执行次数", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            int n = Convert.ToInt32(Parameters[0].Value);

            int loopCount = Convert.ToInt32(Tag) + 1;
            Tag = loopCount;

            if (loopCount <= n)
            {
                int k = 0, k2 = 0, endLoc = 0;

                for (int i = context.CurrentLocation + 1; i < context.ActionCommandList.Count; i++)
                {
                    var command = context.ActionCommandList[i];

                    if (command.Id == "4c0d83ac-bc1f-4f5f-87be-1c242ce1ee24")
                    {
                        k++;
                    }

                    if (command.Id == "e183e929-218d-4471-bb68-191a38cdf00c")
                    {
                        if (k2 == k)
                        {
                            endLoc = i;
                            break;
                        }
                        k2++;
                    }
                }

                var curLoopCmd = new LoopActionCommandModel();
                curLoopCmd.StartLocation = context.CurrentLocation;
                curLoopCmd.EndLocation = endLoc;
                context.LoopActionCommandStack.Push(curLoopCmd);
            }
            else
            {
                Tag = null;
            }
        }
    }
}
