using System;
using System.Collections;
using System.IO;
using System.Text;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class ReadRandomSectionFromFileBySplitAndNoRepeat : ActionCommand
    {
        private Hashtable hashtable = null;

        public ReadRandomSectionFromFileBySplitAndNoRepeat()
        {
            Id = "677da59d-ff35-4ee3-bca4-c844886f2d52";
            Name = "读入文件随机一段不重复内容-按分隔符";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "存放内容的变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "文件路径", ActionParameterType.Text));
            Parameters.Add(new ActionParameterModel(3, "分隔符", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string str = Parameters[1].Value.ToString();
            StreamReader sr = new StreamReader(context.GetCurrentFilePath(str), Encoding.Default);
            string str2 = Parameters[2].Value.ToString();
            str = sr.ReadToEnd();
            sr.Close();

            string[] strArray = str.Split(str2.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Random ran = new Random();
            if (hashtable == null)
                hashtable = new Hashtable();

            context.Cache["ReadRandomSectionFromFileBySplitAndNoRepeat"] = hashtable;

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

            str2 = Parameters[0].Value;

            if (n < strArray.Length)
                context.SetVariableValue(str2, strArray[n], Name);
        }
    }
}
