using System.Collections;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class RstReadRandomLineFromFileAndNoRepeat : ActionCommand
    {
        public RstReadRandomLineFromFileAndNoRepeat()
        {
            Id = "844e0408-19fb-4a39-a8d1-d46b37034915";
            Name = "重置读入文件随机一行不重复内容";
            Type = ActionCommandType.Sys;
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            Hashtable hashtable = context.Cache["ReadRandomLineFromFileAndNoRepeat"] as Hashtable;
            hashtable?.Clear();
        }
    }
}
