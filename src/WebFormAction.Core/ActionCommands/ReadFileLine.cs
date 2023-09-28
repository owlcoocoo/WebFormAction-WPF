using System;
using System.IO;
using System.Text;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class ReadFileLine : ActionCommand
    {
        public ReadFileLine()
        {
            Id = "15bc33b4-f1bb-4549-8119-7af3599bc4ef";
            Name = "读入文件指定一行内容";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "存放内容的变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "文件路径", ActionParameterType.Text));
            Parameters.Add(new ActionParameterModel(3, "第几行", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string str = Parameters[1].Value;
            StreamReader sr = new StreamReader(context.GetCurrentFilePath(str), Encoding.Default);
            string str2 = Parameters[0].Value;
            str = sr.ReadToEnd();
            sr.Close();

            string[] strArray = str.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            str = Parameters[2].Value;
            if (str.Trim() == "")
                str = "1";
            int n = Convert.ToInt32(str);
            n -= 1;

            if (n < strArray.Length)
                context.SetVariableValue(str2, strArray[n], Name);
        }
    }
}
