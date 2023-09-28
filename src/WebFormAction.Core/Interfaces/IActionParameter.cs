using Newtonsoft.Json;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.Interfaces
{
    public interface IActionParameter
    {
        int Id { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        [JsonIgnore]
        ActionParameterType Type { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        dynamic Value { get; set; }
    }
}
