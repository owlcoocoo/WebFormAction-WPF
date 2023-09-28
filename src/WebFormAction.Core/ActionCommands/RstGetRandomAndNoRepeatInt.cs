using System.Collections;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class RstGetRandomAndNoRepeatInt : ActionCommand
    {
        public RstGetRandomAndNoRepeatInt()
        {
            Id = "03f83579-de17-4a61-946c-083be62231a2";
            Name = "重置取随机不重复整数";
            Type = ActionCommandType.Sys;
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            Hashtable hashtable = context.Cache["GetRandomAndNoRepeatInt"] as Hashtable;
            hashtable?.Clear();
        }
    }
}
