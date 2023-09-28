using System.Collections.Generic;
using System.IO;

namespace WebFormAction.Core.Utilities
{
    public class File
    {
        public static string GetFileDir(string path)
        {
            if (path == null)
                return "";
            int pos = path.LastIndexOf('\\');
            if (pos == -1)
                return "";
            return path.Substring(0, pos);
        }

        public static void GetAllFileByDir(string DirPath, string searchPattern, List<string> LI_Files)
        {
            foreach (string file in Directory.GetFiles(DirPath, searchPattern))
                LI_Files.Add(file);

            foreach (string dir in Directory.GetDirectories(DirPath))
                GetAllFileByDir(dir, searchPattern, LI_Files);
        }
    }
}
