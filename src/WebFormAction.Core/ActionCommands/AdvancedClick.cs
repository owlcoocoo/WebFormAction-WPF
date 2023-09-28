using CefSharp;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class AdvancedClick : ActionCommand
    {
        public AdvancedClick()
        {
            Id = "dc747f5b-1f29-4059-a9fc-97d2b5a2224a";
            Name = "高级点击";
            Type = ActionCommandType.Web;
            Parameters.Add(new ActionParameterModel(1, "元素", ActionParameterType.Element));
        }

        public override void Execute(ActionContext context)
        {
            string js;
            Task<JavascriptResponse> t;

            int frameX = 0, frameY = 0;
            string ele = Parameters[0].Value;
            var browser = context.WebBrowser.GetBrowserHost();

            string[] strArray = ele.Split('|');
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

            frameX = 0; frameY = 0;
            frame = context.GetIdentifierFrame(n);
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

            js =
                @"var args1; var ele = getElement(args1);var n = -1;
                  if (!ele.getBoundingClientRect) return 0;
                  ele.getBoundingClientRect().x >= 0 ? n = ele.getBoundingClientRect().x + ele.getBoundingClientRect().width / 2 : n - ele.getBoundingClientRect().x;
                  return n;";
            t = context.RunScript(js, new List<ActionParameterModel>() { new ActionParameterModel(1, "", ActionParameterType.Element) { Value = ele } });
            int x = Convert.ToInt32(t?.Result.Result ?? -1);
            if (x < 0)
            {
                js = "var args1; var ele = getElement(args1);return ele.getBoundingClientRect().right - 1;";
                t = context.RunScript(js, new List<ActionParameterModel>() { new ActionParameterModel(1, "", ActionParameterType.Element) { Value = ele } });
                x = frameX + Convert.ToInt32(t?.Result.Result ?? -1);
            }
            x = x >= 0 ? x + frameX : x;

            js = "var args1; var ele = getElement(args1);if (!ele.getBoundingClientRect) return 0;return ele.getBoundingClientRect().y;";
            t = context.RunScript(js, new List<ActionParameterModel>() { new ActionParameterModel(1, "", ActionParameterType.Element) { Value = ele } });
            int y = Convert.ToInt32(t?.Result.Result ?? -1);
            y = y >= 0 ? y + frameY : y;

            browser.SendMouseClickEvent(x, y, MouseButtonType.Left, false, 1, CefEventFlags.None);
            browser.SendMouseClickEvent(x, y, MouseButtonType.Left, true, 1, CefEventFlags.None);
        }
    }
}
