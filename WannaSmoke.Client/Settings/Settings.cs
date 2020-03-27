using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WannaSmoke.Client
{
    public class Settings
    {
        public Config Config { get; set; }
        public Settings(NameValueCollection appSettings)
        {
            if(!this.ReadConfig())
            {
                this.Config = new Config
                {
                    Name = string.Empty,
                    Server = appSettings["Server"],
                    Hub = appSettings["Hub"],
                    Port = appSettings["Port"]
                };
                this.SaveConfig();
            }
        }
        public bool SaveConfig()
        {
            try
            {
                using (StreamWriter w = new StreamWriter("config.json"))
                {
                    var json = new JsonSerializer();
                    json.Serialize(w, this.Config);
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool ReadConfig()
        {
            if (!File.Exists("config.json"))
            {
                return false;
            }
            else
            {
                using (StreamReader r = new StreamReader("config.json"))
                {
                    string json = r.ReadToEnd();
                    this.Config =  JsonConvert.DeserializeObject<Config>(json);
                }
            }
            return true;
        }
    }
    public class Config
    {
        public string Server { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string Hub { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public String GetFullServer() => String.Join(":", Server, Port);
    }
}
