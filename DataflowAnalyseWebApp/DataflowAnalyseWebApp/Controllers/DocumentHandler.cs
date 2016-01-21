using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Configuration;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataflowAnalyseWebApp.Controllers
{
    public class DocumentHandler
    {
        public DocumentHandler()
        {
            if (!Directory.Exists(WebConfigurationManager.AppSettings["SaveDir"]))
                Directory.CreateDirectory(WebConfigurationManager.AppSettings["SaveDir"]);
        }

        public void Save(string fileName, IEnumerable data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText((WebConfigurationManager.AppSettings["SaveDir"] + @"\" + fileName + ".json"), jsonData);
        }
        public IEnumerable<T> Load<T>(string fileName)
        {
            return ((JArray)JsonConvert.DeserializeObject(File.ReadAllText(WebConfigurationManager.AppSettings["SaveDir"] + @"\" + fileName + ".json"))).ToObject<IEnumerable<T>>();
        }
        public bool Exists(string fileName)
        {
            return File.Exists(WebConfigurationManager.AppSettings["SaveDir"] + @"\" + fileName + ".json");
        }
    }
}