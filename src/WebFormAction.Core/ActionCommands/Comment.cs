using System.Drawing;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class Comment : ActionCommand
    {
        public Comment()
        {
            Id = "28d1a7ee-68ab-4730-8b63-e1209cdb9838";
            Name = "注释";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "要说明的内容", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
            ForeColor = Color.Green;
            BackColor = Color.White;
        }

        public override void Execute(ActionContext context)
        {
        }
    }
}
