using Newtonsoft.Json;
using System.Collections.Generic;
using WebFormAction.Core.Models;

namespace WebFormAction.Core.Interfaces
{
    public interface IActionCommand
    {
        string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        IList<ActionParameterModel> Parameters { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [JsonIgnore]
        int Order { get; set; }

        /// <summary>
        /// 运行计数
        /// </summary>
        [JsonIgnore]
        int RunTimes { get; set; }

        /// <summary>
        /// 最小延时
        /// </summary>
        decimal MinDelay { get; set; }

        /// <summary>
        /// 最大延时
        /// </summary>
        decimal MaxDelay { get; set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        [JsonIgnore]
        object Tag { get; set; }

        /// <summary>
        /// 指令类型
        /// </summary>
        [JsonIgnore]
        ActionCommandType Type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [JsonIgnore]
        int Sort { get; set; }

    }
}
