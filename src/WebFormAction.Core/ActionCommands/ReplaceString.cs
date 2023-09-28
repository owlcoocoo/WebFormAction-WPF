using System.Text.RegularExpressions;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class ReplaceString : ActionCommand
    {
        public ReplaceString()
        {
            Id = "8279823c-3523-4e62-bacd-ed4d5b2ff6a3";
            Name = "内容文本替换";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "要替换文本的变量", ActionParameterType.Variable));
            Parameters.Add(new ActionParameterModel(2, "旧文本", ActionParameterType.Text));
            Parameters.Add(new ActionParameterModel(3, "新文本", ActionParameterType.Text));
            Parameters.Add(new ActionParameterModel(4, "替换次数", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            string varName = Parameters[0].Value;
            string varValue = context.GetVariableValue(varName);
            string oldStr = Parameters[1].Value;
            string newStr = Parameters[2].Value;
            int.TryParse(Parameters[3].Value, out int count);
            string str = "";
            if (count > 0)
            {
                Regex r = new Regex(oldStr);
                str = r.Replace(varValue, newStr, count);
            }
            else
                str = varValue.Replace(oldStr, newStr);
            context.SetVariableValue(varName, str, Name);
        }
    }
}
