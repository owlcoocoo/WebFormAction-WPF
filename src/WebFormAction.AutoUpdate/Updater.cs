using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebAction.AutoUpdate
{
    public class Updater
    {
        const string URL = "";

        string version;

        public Updater(string ver)
        {
            version = ver;
        }

        private bool IsNewest(string curVer, string newVer)
        {
            int.TryParse(curVer.Replace(".", ""), out int cv);
            int.TryParse(newVer.Replace(".", ""), out int nv);
            return nv > cv;
        }

        public bool CheckUpdate()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var result = httpClient.GetAsync(URL).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = result.Content.ReadAsStringAsync().Result;
                    var verInfo = JsonConvert.DeserializeObject<VersionInfo>(json);
                    if (verInfo != null)
                    {
                        return IsNewest(version, verInfo.version);
                    }
                }
            }
            catch { }

            return false;
        }
    }
}
