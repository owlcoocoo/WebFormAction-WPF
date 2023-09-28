using System.IO;
using System.Reflection;
using System.Text;

namespace WebFormAction.Core.Utilities
{
    public class Scripts
    {
        public static string ReadResource(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var streamReader = new StreamReader(assembly.GetManifestResourceStream($"WebFormAction.Core.Scripts.{fileName}"), Encoding.UTF8);
            string result = streamReader.ReadToEnd();
            streamReader.Close();
            return result;
        }
    }
}
