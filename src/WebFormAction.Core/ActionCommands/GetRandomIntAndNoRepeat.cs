using System;
using System.Collections;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class GetRandomIntAndNoRepeat : ActionCommand
    {
        private Hashtable hashtable = null;

        public GetRandomIntAndNoRepeat()
        {
            Id = "3d077a1f-7cf0-48fa-9f0c-47575c4555e6";
            Name = "取随机不重复整数";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "存放数值的变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "最小整数", ActionParameterType.Text));
            Parameters.Add(new ActionParameterModel(3, "最大整数", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string str = Parameters[1].Value;
            string str2 = Parameters[2].Value;

            int n2 = Convert.ToInt32(str);
            int n3 = Convert.ToInt32(str2);

            Random ran = new Random();
            if (hashtable == null)
                hashtable = new Hashtable();

            context.Cache["RandomIntAndNoRepeat"] = hashtable;

            int n = 0;
            int RmNum = n3 - n2;
            for (int i = 0; hashtable.Count <= RmNum; i++)
            {
                if (RmNum == 0)
                {
                    n = n2;
                    break;
                }
                n = ran.Next(n2, n3 + 1);
                if (!hashtable.ContainsValue(n) && n != 0)
                {
                    hashtable.Add(n, n);
                    break;
                }
            }

            str = Parameters[0].Value;
            context.SetVariableValue(str, n.ToString(), Name);
        }
    }
}
