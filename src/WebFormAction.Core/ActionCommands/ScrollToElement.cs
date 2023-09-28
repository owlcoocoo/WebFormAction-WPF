using CefSharp;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class ScrollToElement : ActionCommand
    {
        public ScrollToElement()
        {
            Id = "86983d2a-312b-4f23-a071-9d8b2a291115";
            Name = "滚动到元素";
            Type = ActionCommandType.Web;
            Parameters.Add(new ActionParameterModel(1, "容器元素", ActionParameterType.Element));
            Parameters.Add(new ActionParameterModel(2, "元素", ActionParameterType.Element));
        }

        public override void Execute(ActionContext context)
        {
            int frameX = 0, frameY = 0, scrollX = 0, scrollY = 0;
            string js, str;
            Task<JavascriptResponse> t;

            var browser = context.WebBrowser.GetBrowserHost();

            str = Parameters[1].Value;
            string[] strArray = str.Split('|');
            int n = Convert.ToInt32(strArray[0].Trim('\''));

            List<long> frameIdentifiers = context.WebBrowser.GetBrowser().GetFrameIdentifiers();
            if (n >= frameIdentifiers.Count)
                return;

            IFrame frame = context.GetIdentifierFrame(n);
            while (frame != null && !frame.IsMain)
            {
                js = "frameElement.getBoundingClientRect().x;";
                t = frame.EvaluateScriptAsync(js, "");
                context.CheckRunner(ref t);
                frameX += Convert.ToInt32(t?.Result.Result ?? 0);

                js = "frameElement.getBoundingClientRect().y;";
                t = frame.EvaluateScriptAsync(js, "");
                context.CheckRunner(ref t);
                frameY += Convert.ToInt32(t?.Result.Result ?? 0);

                frame = frame.Parent;
            }

            js = "var args2; var ele = getElement(args2);return ele.getBoundingClientRect().x - ele.getBoundingClientRect().width;";
            t = context.RunScript(js, Parameters);
            scrollX = Convert.ToInt32(t?.Result.Result ?? -1);

            js = "var args2; var ele = getElement(args2);return ele.getBoundingClientRect().y - ele.getBoundingClientRect().height;";
            t = context.RunScript(js, Parameters);
            scrollY = Convert.ToInt32(t?.Result.Result ?? -1);
            /*
            if (frameX < 1)
                webBrowser.SendMouseWheelEvent(0, 0, -scrollX, 0, CefEventFlags.None);
            if (frameY < 1)
                webBrowser.SendMouseWheelEvent(0, 0, 0, -scrollY, CefEventFlags.None);*/

            if (String.IsNullOrEmpty(Parameters[0].Value))
                js = string.Format("document.documentElement.scrollLeft+={0};document.documentElement.scrollTop+={1};", frameX + scrollX, frameY + scrollY);
            else
            {
                js = string.Format("var args1;var ele = getElement(args1);ele.scrollLeft+={0};ele.scrollTop+={1};", frameX + scrollX, frameY + scrollY);
            }
            t = context.RunScript(js, Parameters);
            context.CheckRunner(ref t);

            context.Delay(1, 1);
        }
    }
}
