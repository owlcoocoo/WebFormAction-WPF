using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class RunJavaScript : ActionCommand
    {
        public RunJavaScript()
        {
            Id = "55f9edcd-40be-4ffa-a2db-0bcd47f74d8e";
            Name = "运行JavaScript";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "脚本", ActionParameterType.Text));
            Parameters.Add(new ActionParameterModel(2, "存放返回值的变量", ActionParameterType.Variable));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string js = Parameters[0].Value;
            string varName = Parameters[1].Value;

            var t = context.RunScript(js);

            if (varName.Trim() != "" && t?.Result.Result != null)
                context.SetVariableValue(varName, t.Result.Result.ToString(), Name);
        }
    }
}
