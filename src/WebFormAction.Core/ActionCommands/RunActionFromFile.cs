using WebFormAction.Core.Models;

namespace WebFormAction.Core.ActionCommands
{
    public class RunActionFromFile : ActionCommand
    {
        public RunActionFromFile()
        {
            Id = "4e7b2227-e7ef-4b6d-82c8-1c176a75adce";
            Name = "执行文件动作";
            Type = ActionCommandType.Sys;
            Parameters.Add(new ActionParameterModel(1, "文件路径", ActionParameterType.Text));
            MinDelay = 0;
            MaxDelay = 0;
        }

        public override void Execute(ActionContext context)
        {
            //string filePath = Parameters[0].Value;
            //Stream stream = new FileStream(context.GetCurrentFilePath(filePath), FileMode.Open, FileAccess.Read, FileShare.Read);

            //XmlSerializer formatter = new XmlSerializer(typeof(StreamObject));
            //StreamObject so = (StreamObject)formatter.Deserialize(stream);
            //stream.Close();

            //List<Variable> tmpList = AddVarList(so.VariableList);
            //actionRunner = new ActionRunner(this.chromiumWebBrowser, tmpList);
            //actionRunner.ActionFilePath = this.ActionFilePath;
            //actionRunner.VariableChangedEvent += this.VariableChangedEvent;
            //actionRunner.ActionException += this.ActionException;
            //actionRunner.Run(so.ActionList).Wait();
            //if (actionRunner.IsStop)
            //    this.Stop();

            //for (int i = 0; i < actionRunner.varList.Count; i++)
            //    this.varList[i].Content = actionRunner.varList[i].Content;

            //this.hashtable_eleClick = actionRunner.hashtable_eleClick;
            //this.hashtable_rand = actionRunner.hashtable_rand;
            //this.hashtable_randFile = actionRunner.hashtable_randFile;
            //this.hashtable_randFileRead = actionRunner.hashtable_randFileRead;
            //this.hashtable_randFileRead2 = actionRunner.hashtable_randFileRead2;
        }
    }
}
