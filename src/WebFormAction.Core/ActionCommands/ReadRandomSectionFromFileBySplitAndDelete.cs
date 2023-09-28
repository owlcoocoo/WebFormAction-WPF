using System;
using System.IO;
using System.Text;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class ReadRandomSectionFromFileBySplitAndDelete : ActionCommand
    {
        public ReadRandomSectionFromFileBySplitAndDelete()
        {
            Id = "34a3bdb8-d354-4002-8e25-3a4a85805337";
            Name = "读入文件随机一段内容并删除-按分隔符";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "存放内容的变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "文件路径", ActionParameterType.Text));
            Parameters.Add(new ActionParameterModel(3, "分隔符", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string str = Parameters[1].Value;
            StreamReader sr = new StreamReader(context.GetCurrentFilePath(str), Encoding.Default);
            string str2 = Parameters[2].Value;
            str = sr.ReadToEnd();
            sr.Close();

            string[] strArray = str.Split(str2.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Random ran = new Random();
            int n = ran.Next(0, strArray.Length);

            str2 = Parameters[0].Value;

            if (n < strArray.Length)
            {
                context.SetVariableValue(str2, strArray[n], Name);

                str2 = Parameters[2].Value;
                str = "";
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (i != n)
                        str += strArray[i] + str2;
                }
                if (str.Length > 0)
                    str = str.Substring(0, str.Length - str2.Length);
                str2 = context.GetCurrentFilePath(Parameters[1].Value);
                StreamWriter sw = new StreamWriter(str2, false, Encoding.Default);
                sw.Write(str);
                sw.Close();
            }
        }
    }
}
