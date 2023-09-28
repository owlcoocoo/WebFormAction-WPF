using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;

namespace WebFormAction.Core.Models
{
    public enum ActionCommandType { None, Web, Sys };

    public class ActionCommandModel
    {
        private const char SplitChar = '\x7f';

        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public IList<ActionParameterModel> Parameters { get; set; } = new List<ActionParameterModel>();

        /// <summary>
        /// 显示顺序
        /// </summary>
        [JsonIgnore]
        public int Order { get; set; }

        /// <summary>
        /// 运行计数
        /// </summary>
        [JsonIgnore]
        public int RunTimes { get; set; }

        /// <summary>
        /// 最小延时
        /// </summary>
        public decimal MinDelay { get; set; } = 0.5m;

        /// <summary>
        /// 最大延时
        /// </summary>
        public decimal MaxDelay { get; set; } = 1;

        /// <summary>
        /// 前景色
        /// </summary>
        [JsonIgnore]
        public Color ForeColor { get; set; } = Color.Black;

        /// <summary>
        /// 背景色
        /// </summary>
        [JsonIgnore]
        public Color BackColor { get; set; } = Color.White;

        /// <summary>
        /// 自定义数据
        /// </summary>
        [JsonIgnore]
        public object Tag { get; set; }

        /// <summary>
        /// 指令类型
        /// </summary>
        [JsonIgnore]
        public ActionCommandType Type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [JsonIgnore]
        public int Sort { get; set; }

        public virtual string GetParameterString()
        {
            string retStr = "";
            foreach (var item in this.Parameters)
            {
                retStr += item.Value.ToString() + SplitChar;
            }
            if (retStr.Length > 0)
                retStr = retStr.Substring(0, retStr.Length - 1);

            return retStr;
        }
    }
}
