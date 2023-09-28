using CefSharp;
using System;
using System.Threading.Tasks;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class WebScroll : ActionCommand
    {
        public WebScroll()
        {
            Id = "9227f7c0-e7b3-4f95-bbe1-0a8a43affe42";
            Name = "网页滚动";
            Type = ActionCommandType.Web;
            Parameters.Add(new ActionParameterModel(1, "元素", ActionParameterType.Element));
            Parameters.Add(new ActionParameterModel(2, "水平滚动数值", ActionParameterType.Text));
            Parameters.Add(new ActionParameterModel(3, "垂直滚动数值", ActionParameterType.Text));
        }

        public override void Execute(ActionContext context)
        {
            string ele = Parameters[0].Value;
            string js = "var args1; var ele = getElement(args1);return ele.getBoundingClientRect().x;";
            Task<JavascriptResponse> t = context.RunScript(js, Parameters);
            int x = Convert.ToInt32(t?.Result.Result ?? 0);

            js = "var args1; var ele = getElement(args1);return ele.getBoundingClientRect().y;";
            t = context.RunScript(js, Parameters);
            int y = Convert.ToInt32(t?.Result.Result ?? 0);
            string xValue = Parameters[1].Value;
            if (xValue.Trim() == "")
                xValue = "0";
            string yValue = Parameters[2].Value;
            if (yValue.Trim() == "")
                yValue = "0";
            int deltaX = Convert.ToInt32(xValue);
            int deltaY = Convert.ToInt32(yValue);

            context.WebBrowser.SendMouseWheelEvent(x, y, -deltaX, 0, CefEventFlags.None);
            context.WebBrowser.SendMouseWheelEvent(x, y, 0, -deltaY, CefEventFlags.None);
        }
    }
}
