using System;
using System.IO;
using System.Text;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class ReadRandomLineFromFileAndDelete : ActionCommand
    {
        public ReadRandomLineFromFileAndDelete()
        {
            Id = "8ed2c27f-48a8-48fc-aefb-85f5da2485b5";
            Name = "读入文件随机一行内容并删除";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "存放内容的变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "文件路径", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string str = Parameters[1].Value.ToString();
            StreamReader sr = new StreamReader(context.GetCurrentFilePath(str), Encoding.Default);
            string str2 = Parameters[0].Value.ToString();
            str = sr.ReadToEnd();
            sr.Close();

            string[] strArray = str.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Random ran = new Random();
            int n = ran.Next(0, strArray.Length);

            if (n < strArray.Length)
            {
                context.SetVariableValue(str2, strArray[n], Name);

                str = "";
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (i != n)
                        str += strArray[i] + "\r\n";
                }
                if (str.Length > 0)
                    str = str.Substring(0, str.Length - 2);
                str2 = context.GetCurrentFilePath(Parameters[1].Value);
                StreamWriter sw = new StreamWriter(str2, false, Encoding.Default);
                sw.Write(str);
                sw.Close();
            }
        }
    }
}
