using CefSharp;

using System;
using System.Collections;
using System.Threading.Tasks;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class RandomClickByNoRepeat : ActionCommand
    {
        private Hashtable hashtable = null;

        public RandomClickByNoRepeat()
        {
            Id = "447e54f1-162f-4512-b10a-11010bb2be85";
            Name = "随机点击-不重复";
            Type = ActionCommandType.Web;
            Parameters.Add(new ActionParameterModel(1, "父元素", ActionParameterType.Element));
        }

        public override void Execute(ActionContext context)
        {
            string js = "var args1;var ele=getElement(args1);return ele.children.length;";
            Task<JavascriptResponse> t = context.RunScript(js, Parameters);
            if (t?.Result.Result != null)
            {
                int RmNum = Convert.ToInt32(t.Result.Result) - 1;

                Random ran = new Random();
                if (hashtable == null)
                    hashtable = new Hashtable();

                context.Cache["RandomClickByNoRepeat"] = hashtable;

                int n = 0;
                for (int i = 0; hashtable.Count < RmNum; i++)
                {
                    n = ran.Next(1, RmNum + 1);
                    if (!hashtable.ContainsValue(n) && n != 0)
                    {
                        hashtable.Add(n, n);
                        break;
                    }
                }

                js = "var n=" + n.ToString() + ";var args1;var ele=getElement(args1);ele=ele.children[n];while(ele.firstElementChild)ele=ele.firstElementChild;ele.click();";
                context.RunScriptAndNoResult(js, Parameters);
            }
        }
    }
}
