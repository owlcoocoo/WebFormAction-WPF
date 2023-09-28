using WebFormAction.Core.Models;

namespace WebFormAction.Core
{
    public abstract class ActionCommand : ActionCommandModel
    {
        public abstract void Execute(ActionContext context);
    }
}
