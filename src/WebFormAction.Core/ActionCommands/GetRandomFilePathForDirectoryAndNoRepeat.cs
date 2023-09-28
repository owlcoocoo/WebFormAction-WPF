using System;
using System.Collections;
using System.Collections.Generic;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class GetRandomFilePathForDirectoryAndNoRepeat : ActionCommand
    {
        private Hashtable hashtable = null;

        public GetRandomFilePathForDirectoryAndNoRepeat()
        {
            Id = "cd0be341-2a67-498b-a104-9e7336bb5efb";
            Name = "取指定目录下随机不重复的一个文件路径";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "存放内容的变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "目录", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string str = Parameters[0].Value;
            string str2 = context.GetCurrentFilePath(Parameters[1].Value);
            var files = new List<string>();
            Utilities.File.GetAllFileByDir(str2, "*.*", files);

            var ran = new Random();
            if (hashtable == null)
                hashtable = new Hashtable();

            context.Cache["GetRandomFilePathForDirectoryAndNoRepeat"] = hashtable;

            int n = 0;
            int RmNum = files.Count - 1;
            for (int i = 0; hashtable.Count < RmNum; i++)
            {
                n = ran.Next(0, RmNum + 1);
                if (!hashtable.ContainsValue(n) && n != 0)
                {
                    hashtable.Add(n, n);
                    break;
                }
            }

            if (n < files.Count)
                context.SetVariableValue(str, files[n], Name);
        }
    }
}
