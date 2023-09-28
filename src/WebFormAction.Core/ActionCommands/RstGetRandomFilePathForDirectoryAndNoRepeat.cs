using System.Collections;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class RstGetRandomFilePathForDirectoryAndNoRepeat : ActionCommand
    {
        public RstGetRandomFilePathForDirectoryAndNoRepeat()
        {
            Id = "23fae1d1-428a-47d2-af34-5381cd0f5e04";
            Name = "重置取指定目录下随机不重复的一个文件路径";
            Type = ActionCommandType.Sys;
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            Hashtable hashtable = context.Cache["GetRandomFilePathForDirectoryAndNoRepeat"] as Hashtable;
            hashtable?.Clear();
        }
    }
}
