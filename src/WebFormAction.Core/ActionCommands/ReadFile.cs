using System.IO;
using System.Text;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class ReadFile : ActionCommand
    {
        public ReadFile()
        {
            Id = "e99dcc48-869f-4e71-b3d9-bc5e2ad69be8";
            Name = "读入文件内容";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "存放内容的变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "文件路径", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string str = Parameters[1].Value;
            StreamReader sr = new StreamReader(context.GetCurrentFilePath(str), Encoding.Default);
            str = Parameters[0].Value;
            context.SetVariableValue(str, sr.ReadToEnd(), Name);
            sr.Close();
        }
    }
}
