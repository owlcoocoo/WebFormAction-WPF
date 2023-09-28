using System.Threading.Tasks;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class WaitForWebLoading : ActionCommand
    {
        public WaitForWebLoading()
        {
            Id = "6fc99683-475f-40ff-820f-686136c8997e";
            Name = "等待网页载入完毕";
            Type = ActionCommandType.Web;
            Parameters.Add(new ActionParameterModel(1, "匹配的网址文本", ActionParameterType.Text));
        }

        public override void Execute(ActionContext context)
        {
            while (context.WebBrowser.Address.IndexOf(Parameters[0].Value) == -1)
            {
                Task.Delay(1000).Wait();
                if (context.IsStop)
                    break;
            }
        }
    }
}
