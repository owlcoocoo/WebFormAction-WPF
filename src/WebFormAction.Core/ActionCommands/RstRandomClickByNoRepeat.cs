using System.Collections;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class RstRandomClickByNoRepeat : ActionCommand
    {
        public RstRandomClickByNoRepeat()
        {
            Id = "8e13ccce-ad50-4599-8ac1-b357bfb8d1da";
            Name = "重置随机点击-不重复";
            Type = ActionCommandType.Sys;
        }

        public override void Execute(ActionContext context)
        {
            Hashtable hashtable = context.Cache["RandomClickByNoRepeat"] as Hashtable;
            hashtable?.Clear();
        }
    }
}
