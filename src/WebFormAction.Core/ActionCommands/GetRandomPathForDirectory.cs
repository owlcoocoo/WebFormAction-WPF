using System;
using System.Collections.Generic;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class GetRandomPathForDirectory : ActionCommand
    {
        public GetRandomPathForDirectory()
        {
            Id = "b79b2130-5576-4b6e-957f-1beb6a4638e1";
            Name = "取指定目录下的随机一个文件路径";
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
            List<string> files = new List<string>();
            Utilities.File.GetAllFileByDir(str2, "*.*", files);

            Random ran = new Random();
            int n = ran.Next(0, files.Count);

            if (n < files.Count)
                context.SetVariableValue(str, files[n], Name);
        }
    }
}
