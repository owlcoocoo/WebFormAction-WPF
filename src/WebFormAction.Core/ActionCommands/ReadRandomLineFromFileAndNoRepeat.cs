using System;
using System.Collections;
using System.IO;
using System.Text;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class ReadRandomLineFromFileAndNoRepeat : ActionCommand
    {
        private Hashtable hashtable = null;

        public ReadRandomLineFromFileAndNoRepeat()
        {
            Id = "b50d3d3a-b4a7-4d01-8b89-0579268e14bc";
            Name = "读入文件随机一行不重复内容";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "存放内容的变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "文件路径", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string str = Parameters[1].Value;
            StreamReader sr = new StreamReader(context.GetCurrentFilePath(str), Encoding.UTF8);
            string str2 = Parameters[0].Value;
            str = sr.ReadToEnd();
            sr.Close();

            string[] strArray = str.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            Random ran = new Random();
            if (hashtable == null)
                hashtable = new Hashtable();

            context.Cache["ReadRandomLineFromFileAndNoRepeat"] = hashtable;

            int n = 0;
            int RmNum = strArray.Length - 1;
            for (int i = 0; hashtable.Count < RmNum; i++)
            {
                n = ran.Next(0, RmNum + 1);
                if (!hashtable.ContainsValue(n) && n != 0)
                {
                    hashtable.Add(n, n);
                    break;
                }
            }

            if (n < strArray.Length)
                context.SetVariableValue(str2, strArray[n], Name);
        }
    }
}
