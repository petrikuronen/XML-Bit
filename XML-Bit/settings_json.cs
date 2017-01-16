using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace XML_Bit
{
    class settings_json
    {
        public void writesettings(string filename,List<machine> m)
        {
            string output = JsonConvert.SerializeObject(m);

            try
            {
                if (File.Exists(@".\settings.json"))
                {
                    System.IO.File.WriteAllText(@".\settings.json", string.Empty);
                    Console.WriteLine("Raderad?");
                }
            }
            catch
            {
                Console.WriteLine("radera = failed");
            }
            StreamWriter writer = File.CreateText(@".\settings.json");
            writer.WriteLine(output);
            writer.Flush();
            writer.Close();
        }
        public List<machine>loadsettings(List<machine> m)
        {
            using (StreamReader r = new StreamReader("settings.json"))
            {
                string json = r.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(json);
                int i = 0;
                foreach (var item in array)
                {
                    m.Add(new machine(Convert.ToString(item.id), Convert.ToInt32(item.ports), Convert.ToInt32(item.counters)));
                    i++;
                }
            }
            return m;
        }

    }
}
