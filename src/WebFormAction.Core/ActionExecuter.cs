using CefSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebFormAction.Core.Interfaces;
using WebFormAction.Core.Models;

namespace WebFormAction.Core
{
    public class ActionExecuter
    {
        private const int sleepDelay = 100;

        public delegate void ExecuteCommandDelegate(ActionContext context, ActionCommand cmd);
        public event ExecuteCommandDelegate BeforeExecuteCommandEvent;
        public event ExecuteCommandDelegate AfterExecuteCommandEvent;

        public event Action ActionEndEvent;
        public event Action<Exception> ActionExceptionEvent;

        public ActionContext Context { get; }

        public delegate dynamic InvokeDispatcher(Func<dynamic> action);
        private readonly InvokeDispatcher dispatcher;

        public ActionExecuter(IWebBrowser webBrowser, IList<IActionVariable> actionVariableList, InvokeDispatcher dispatcher)
        {
            Context = new ActionContext(webBrowser, actionVariableList);
            this.dispatcher = dispatcher;
        }

        public bool IsStop => Context.IsStop;
        public bool IsPause => Context.IsPause;

        public void Stop() => Context.Stop();
        public void Pause() => Context.Pause();
        public void Continue() => Context.Continue();

        public static List<ActionCommand> GetCommandList()
        {
            return new ActionContext(null, null).CommandList;
        }

        private void ExecuteCommand(ActionCommand command)
        {
            BeforeExecuteCommandEvent?.Invoke(Context, command);

            Context.SetVariableValue(command.Parameters);
            command.Execute(Context);

            // 30秒等待超时
            var cancellationTokenSource = new CancellationTokenSource(30000);
            var taskTimeout = Task.Run(() =>
            {
                bool isLoading = true;
                while (isLoading)
                {
                    if (cancellationTokenSource.IsCancellationRequested)
                        break;

                    Task.Delay(sleepDelay).Wait();
                    if (Context.IsStop)
                        break;

                    var result = dispatcher.Invoke(() =>
                    {
                        return Context.WebBrowser.IsLoading;
                    });
                    if (result != null) isLoading = result;
                }
            });
            taskTimeout.Wait();

            if (Context.IsStop)
                return;

            Context.Delay(command.MinDelay, command.MaxDelay);

            AfterExecuteCommandEvent?.Invoke(Context, command);
        }

        public Task Execute(List<ActionCommandModel> actionCommandList)
        {
            Context.ActionCommandList = Context.GetActionCommandList(actionCommandList);

            Task t = new Task(() =>
            {
                ActionCommand command = null;

                try
                {
                    for (Context.CurrentLocation = 0; Context.CurrentLocation < Context.ActionCommandList.Count; Context.CurrentLocation++)
                    {
                        command = Context.ActionCommandList[Context.CurrentLocation];
                        ExecuteCommand(command);

                        if (Context.NextCurrentLocation >= 0)
                        {
                            Context.CurrentLocation = Context.NextCurrentLocation - 1;
                            Context.NextCurrentLocation = -1;
                        }

                        while (Context.IsPause || Context.IsStop)
                        {
                            Task.Delay(sleepDelay).Wait();
                            if (Context.IsStop)
                                return;
                        }
                    }

                    ActionEndEvent?.Invoke();
                }
                catch (Exception ex)
                {
                    Stop();
                    ActionExceptionEvent?.Invoke(new Exception(ex.Message, new Exception(ex.ToString() + "\r\n" + command?.Name + "\r\n" + command?.GetParameterString())));
                    return;
                }

            });
            t.Start();

            return t;
        }
    }
}
