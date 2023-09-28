using Newtonsoft.Json;
using WebFormAction.Core.Interfaces;

namespace WebFormAction.Core.Models
{
    public enum ActionParameterType { None, Text, Variable, Element };

    /// <summary>
    /// 动作参数
    /// </summary>
    public class ActionParameterModel : IActionParameter
    {
        public int Id { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        [JsonIgnore]
        public ActionParameterType Type { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public dynamic Value { get; set; } = "";


        public ActionParameterModel(int id, string name, ActionParameterType paramType)
        {
            Id = id;
            Name = name;
            Type = paramType;
        }
    }
}
