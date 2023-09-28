using System;
using System.IO;
using System.Text;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class ReadRandomLineFromFile : ActionCommand
    {
        public ReadRandomLineFromFile()
        {
            Id = "aa543664-d928-49df-81a2-5a8ab0824dd4";
            Name = "读入文件随机一行内容";
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
            string str2 = Parameters[0].Value;
            str = sr.ReadToEnd();
            sr.Close();

            string[] strArray = str.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Random ran = new Random();
            int n = ran.Next(0, strArray.Length);

            if (n < strArray.Length)
                context.SetVariableValue(str2, strArray[n], Name);
        }
    }
}
