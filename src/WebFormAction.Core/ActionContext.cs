using CefSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WebFormAction.Core.Extensions;
using WebFormAction.Core.Interfaces;
using WebFormAction.Core.Models;

namespace WebFormAction.Core
{
    public class ActionContext
    {
        #region 默认参数

        private const int SLEEP_DELAY = 100;
        private const string variableRegex = @"\$\((.+?)\)\$";

        #endregion

        #region 公共变量

        public string ActionFilePath { get; set; }
        public int CurrentLocation { get; set; }
        public int NextCurrentLocation { get; set; } = -1;
        public int LoopTimes { get; set; }
        public bool IsPause { get; private set; }
        public bool IsStop { get; private set; }

        private string CommonScript { get; }

        public List<ActionCommand> CommandList { get; set; }
        public Stack<LoopActionCommandModel> LoopActionCommandStack { get; set; } = new Stack<LoopActionCommandModel>();

        public Dictionary<string, object> Cache { get; } = new Dictionary<string, object>();

        #endregion

        #region 公共事件

        public delegate void VariableChanged(string varName, string varValue, string cmdName, int line);
        public event VariableChanged VariableChangedEvent;

        #endregion

        #region 构造方法

        public IWebBrowser WebBrowser { get; }
        public IList<IActionVariable> ActionVariableList { get; set; }
        public List<ActionCommand> ActionCommandList { get; set; }


        public ActionContext(IWebBrowser webBrowser, IList<IActionVariable> actionVariableList)
        {
            WebBrowser = webBrowser;
            ActionVariableList = actionVariableList;

            CommonScript = Utilities.Scripts.ReadResource("Common.js");
            CommandList = GetAllCommands().ToList();
        }

        #endregion

        public void Stop()
        {
            IsStop = true;
        }

        public void Pause()
        {
            IsPause = true;
        }

        public void Continue()
        {
            IsPause = false;
        }

        private IEnumerable<ActionCommand> GetAllCommands()
        {
            var asmTypes = Assembly.Load("WebFormAction.Core").GetTypes();
            var baseType = typeof(ActionCommand);
            var commandTypes = asmTypes.Where(a => a.BaseType == baseType);
            foreach (var type in commandTypes)
            {
                var command = Activator.CreateInstance(type) as ActionCommand;
                yield return command;
            }
        }

        public ActionCommand GetActionCommand(ActionCommandModel model)
        {
            var command = CommandList.First(a => a.Name == model.Name);
            var newCommand = Activator.CreateInstance(command.GetType()) as ActionCommand;
            newCommand.Parameters = model.Parameters;
            return newCommand;
        }

        public List<ActionCommand> GetActionCommandList(List<ActionCommandModel> modelList)
        {
            var list = modelList.Clone();
            var commandList = new List<ActionCommand>();
            foreach (var item in list)
            {
                var command = GetActionCommand(item);
                commandList.Add(command);
            }

            return commandList;
        }

        public string GetCurrentFilePath(string checkPath)
        {
            string path = Utilities.File.GetFileDir(ActionFilePath);
            if (path != "")
            {
                if (checkPath.IndexOf(":") != -1)
                {
                    path = checkPath;
                }
                else
                {
                    path += checkPath;
                }
            }
            else
                path = checkPath;

            return path;
        }

        public string GetVariableValue(string name)
        {
            name = name.Substring(2, name.Length - 4);
            for (int i = 0; i < ActionVariableList.Count; i++)
            {
                if (name == ActionVariableList[i].Name)
                {
                    return ActionVariableList[i].Value;
                }
            }

            return "";
        }

        public void SetVariableValue(IList<ActionParameterModel> parameters)
        {
            foreach (var item in parameters)
            {
                if (item.Type != ActionParameterType.Variable)
                {
                    var matches = Regex.Matches(item.Value, variableRegex);
                    foreach (var matche in matches)
                    {
                        string varName = matche.ToString();
                        varName = varName.Substring(2, varName.Length - 4);
                        item.Value = item.Value.Replace($"$({varName})$",
                        ActionVariableList.FirstOrDefault(o => o.Name == varName)?.Value);
                    }
                }
            }
        }

        public void SetVariableValue(string name, string value, string cmdName)
        {
            var matches = Regex.Matches(name, variableRegex);
            foreach (var matche in matches)
            {
                string varName = matche.ToString();
                varName = varName.Substring(2, varName.Length - 4);

                for (int i = 0; i < ActionVariableList.Count; i++)
                {
                    if (varName == ActionVariableList[i].Name)
                    {
                        ActionVariableList[i].Value = value;
                        VariableChangedEvent?.Invoke(varName, value, cmdName, CurrentLocation);
                        break;
                    }
                }
            }
        }

        private string SetScriptArgsValue(string script, int index, string value)
        {
            string str = script, str1, str2;
            int pos, pos1, pos2 = 0;

            while (true)
            {
                pos1 = script.IndexOf("var", pos2);
                if (pos1 == -1)
                    break;
                pos2 = script.IndexOf(";", pos1);
                if (pos2 != -1)
                {
                    str1 = script.Substring(pos1, pos2 - pos1);
                    str2 = "args" + index.ToString();
                    pos = str1.IndexOf(str2);
                    if (pos != -1)
                    {
                        pos = script.IndexOf(str2, pos1);
                        pos += str2.Length;

                        str = script.Insert(pos, "=" + value);
                        break;
                    }

                }

            }

            return str;
        }

        public void CheckRunner(ref Task<JavascriptResponse> task)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            task.Wait(3000, cancellationTokenSource.Token);
            if (task.Status == TaskStatus.WaitingForActivation)
            {
                cancellationTokenSource.Cancel();
                task = null;
                //MessageBox.Show("浏览器发生异常！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public IFrame GetIdentifierFrame(int n)
        {
            List<long> frameIdentifiers = WebBrowser.GetBrowser().GetFrameIdentifiers();
            //string js = @"
            //            (function () {
            //                var result = [];
            //                function getAllFrames(targetFrames, index) {
            //                 if (!targetFrames)
            //                  targetFrames = window.frames;
            //                    if (!index) index = 0;
            //                 for (var i = 0; i < targetFrames.length; i++) {
            //                        try {
            //                      var style = targetFrames[i].frameElement.style['display'];
            //                            if (style && style.toLowerCase() == 'none') {
            //                       result.push(index + i + 1);
            //                      }
            //                      getAllFrames(targetFrames[i].frameElement.contentWindow.frames, i + 1);
            //                        }
            //                        catch(error) { }
            //                 }
            //                }
            //                getAllFrames();
            //                return result;
            //            })();";
            //Task<JavascriptResponse> task = webBrowser.GetMainFrame().EvaluateScriptAsync(js, "");
            //task.Wait(3000);
            //var invalidFrameIdentifiers = (List<object>)task.Result?.Result;
            //if (invalidFrameIdentifiers != null)
            //{
            //    invalidFrameIdentifiers.Sort();
            //    for (int i = invalidFrameIdentifiers.Count - 1; i >= 0; i--)
            //    {
            //        frameIdentifiers.RemoveAt(Convert.ToInt32(invalidFrameIdentifiers[i]));
            //    }
            //}
            if (n < frameIdentifiers.Count)
                return WebBrowser.GetBrowser().GetFrame(frameIdentifiers[n]);

            return null;
        }

        private IFrame GetScript(ref string script, IList<ActionParameterModel> parameters)
        {
            IFrame frame = WebBrowser.GetMainFrame();

            if (parameters != null && parameters.Count > 0)
            {
                for (int i = 0; i < parameters.Count; i++)
                {
                    if (parameters[i].Type == ActionParameterType.Variable)
                        continue;

                    string value = parameters[i].Value;
                    value = value.Replace("\r", "\\r").Replace("\n", "\\n");
                    value = value.Replace("'", "\\'");

                    value = $"\'{ value }\'";
                    script = SetScriptArgsValue(script, i + 1, value);
                }

                script = CommonScript + "(function (){" + script + "})();";

                var eleParameter = parameters.FirstOrDefault(o => o.Type == ActionParameterType.Element);
                if (eleParameter == null || eleParameter.Value.ToString().Length < 1)
                    goto RET;
                string[] arry = eleParameter.Value.ToString().Split('|');
                int n = Convert.ToInt32(arry[0].Replace("'", ""));
                frame = GetIdentifierFrame(n);
                if (frame == null)
                    frame = WebBrowser.GetMainFrame();
            }

        RET:

            return frame;
        }

        public void Delay(decimal min, decimal max)
        {
            min *= 1000;
            max *= 1000;

            Random ran = new Random();
            int time = ran.Next((int)min, (int)max + 1);

            if (time <= 0)
                return;

            Task.Delay(time).Wait();
        }

        public void RunScriptAndNoResult(string script, IList<ActionParameterModel> parameters = null)
        {
            var frame = GetScript(ref script, parameters);
            frame.ExecuteJavaScriptAsync(script, "");
        }

        public Task<JavascriptResponse> RunScript(string script, IList<ActionParameterModel> parameters = null)
        {
            Task<JavascriptResponse> task = GetScript(ref script, parameters).EvaluateScriptAsync(script, "");
            CheckRunner(ref task);
            return task;
        }
    }
}
