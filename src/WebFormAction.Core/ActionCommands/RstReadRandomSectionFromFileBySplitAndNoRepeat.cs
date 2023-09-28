using System.Collections;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class RstReadRandomSectionFromFileBySplitAndNoRepeat : ActionCommand
    {
        public RstReadRandomSectionFromFileBySplitAndNoRepeat()
        {
            Id = "33a3d024-51b5-47e6-8a3e-cfc838bb9a2f";
            Name = "重置读入文件随机一段不重复内容-按分隔符";
            Type = ActionCommandType.Sys;
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            Hashtable hashtable = context.Cache["ReadRandomSectionFromFileBySplitAndNoRepeat"] as Hashtable;
            hashtable?.Clear();
        }
    }
}
