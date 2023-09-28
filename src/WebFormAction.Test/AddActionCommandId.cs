using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace WebFormAction.Test
{
    [TestClass]
    public class AddActionCommandId
    {
        [TestMethod]
        public void TestMethod1()
        {
            string path = @"..\..\..\WebFormAction.Core\ActionCommands";
            var filePathList = Directory.EnumerateFiles(path, "*.cs");
            foreach (string filePath in filePathList)
            {
                var fileText = File.ReadAllText(filePath, Encoding.UTF8);
                int pos = fileText.IndexOf("Name =");
                string code = $"Id = \"{Guid.NewGuid()}\";\r\n            ";
                fileText = fileText.Insert(pos, code);
                //File.WriteAllText(filePath, fileText, Encoding.UTF8);
            }
        }
    }
}
