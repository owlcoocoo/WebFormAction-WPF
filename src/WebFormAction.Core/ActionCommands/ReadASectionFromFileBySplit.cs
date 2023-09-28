using System;
using System.IO;
using System.Text;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class ReadASectionFromFileBySplit : ActionCommand
    {
        public ReadASectionFromFileBySplit()
        {
            Id = "1aea1e5f-5d07-4b68-809f-bff3d7a62bf3";
            Name = "读入文件指定一段内容-按分隔符";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "存放内容的变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "文件路径", ActionParameterType.Text));
            Parameters.Add(new ActionParameterModel(3, "分隔符", ActionParameterType.Text));
            Parameters.Add(new ActionParameterModel(4, "第几段", ActionParameterType.Text));
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
            str = Parameters[3].Value;
            if (str.Trim() == "")
                str = "1";
            int n = Convert.ToInt32(str);
            n -= 1;

            str2 = Parameters[0].Value;

            if (n < strArray.Length)
                context.SetVariableValue(str2, strArray[n], Name);
        }
    }
}
