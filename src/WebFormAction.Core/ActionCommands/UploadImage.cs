using System.Collections.Generic;
using WebFormAction.Core.Handlers;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class UploadImage : ActionCommand
    {
        public UploadImage()
        {
            Id = "6aeb399c-c621-4a3c-bcbc-01e818985069";
            Name = "图片上传";
            Type = ActionCommandType.Web;
            Parameters.Add(new ActionParameterModel(1, "元素_上传按钮", ActionParameterType.Element));
            Parameters.Add(new ActionParameterModel(2, "元素_确定按钮", ActionParameterType.Element));
            Parameters.Add(new ActionParameterModel(3, "图片路径", ActionParameterType.Text));
        }

        public override void Execute(ActionContext context)
        {
            string filePath = Parameters[2].Value;
            var fileList = new List<string> { filePath };

            if (context.WebBrowser.DialogHandler == null)
                context.WebBrowser.DialogHandler = new FileDialogHandler(fileList);

            string ele = Parameters[0].Value;
            var advClickCmd = new AdvancedClick();
            advClickCmd.Parameters[0].Value = ele;
            advClickCmd.Execute(context);

            context.Delay(1, 1);

            ele = Parameters[1].Value;
            if (ele.Trim('\'') != "")
            {
                var clickCmd = new Click();
                clickCmd.Parameters[0].Value = ele;
                clickCmd.Execute(context);
            }

            context.Delay(6, 6);

            context.WebBrowser.DialogHandler = null;
        }
    }
}
