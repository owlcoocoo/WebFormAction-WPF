using CefSharp;
using System.Threading.Tasks;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class GetElementContent : ActionCommand
    {
        public GetElementContent()
        {
            Id = "c0e8771c-9982-42a0-b952-42150d280ddc";
            Name = "取元素内容";
            Type = ActionCommandType.Web;
            Parameters.Add(new ActionParameterModel(1, "存放内容的变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "元素", ActionParameterType.Element));
        }

        public override void Execute(ActionContext context)
        {
            string js =
                @"  
                    var args1,args2;
                    var ele = getElement(args2);
                    if (ele.nodeName.toUpperCase() === 'BODY' || ele.nodeName.toUpperCase() === 'DIV')
                    {return ele.innerHTML;} else {if(ele.value) return ele.value; else return ele.innerText;}";
            Task<JavascriptResponse> t = context.RunScript(js, Parameters);
            if (t?.Result.Result != null)
            {
                string str = Parameters[0].Value;
                context.SetVariableValue(str, t.Result.Result.ToString(), Name);
            }
        }
    }
}
