using CefSharp;
using System.Collections.Generic;

namespace WebFormAction.Core.Handlers
{
    public class FileDialogHandler : IDialogHandler
    {
        private readonly List<string> fileList;

        public FileDialogHandler(List<string> selectedFileList)
        {
            fileList = selectedFileList;
        }

        public bool OnFileDialog(IWebBrowser chromiumWebBrowser, IBrowser browser, CefFileDialogMode mode, CefFileDialogFlags flags, string title, string defaultFilePath, List<string> acceptFilters, int selectedAcceptFilter, IFileDialogCallback callback)
        {
            if (fileList.Count > 0)
            {
                callback.Continue(selectedAcceptFilter, fileList);
            }

            return false;
        }
    }
}
