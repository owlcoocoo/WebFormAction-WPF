using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class GetCurrentLoopIndex : ActionCommand
    {
        public GetCurrentLoopIndex()
        {
            Id = "3b297f9e-7e81-43f0-9224-f86b8183ca8a";
            Name = "取当前循环计数";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "欲存放的变量", ActionParameterType.Variable));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string varName = Parameters[0].Value;
            string value = (context.LoopTimes + 1).ToString();
            context.SetVariableValue(varName, value, Name);
        }
    }
}
