using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class EndLoop : ActionCommand
    {
        public EndLoop()
        {
            Id = "e183e929-218d-4471-bb68-191a38cdf00c";
            Name = "结束循环";
            Type = ActionCommandType.Sys;
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            if (context.LoopActionCommandStack.Count > 0)
            {
                var curLoopCmd = context.LoopActionCommandStack.Peek();
                if (context.CurrentLocation == curLoopCmd.EndLocation)
                {
                    context.LoopActionCommandStack.Pop();
                    context.NextCurrentLocation = curLoopCmd.StartLocation;
                }
            }
        }
    }
}
