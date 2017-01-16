using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.IO;
using System.Xml.XPath;
using System.Xml.Linq;
using Newtonsoft.Json;
using S7.Net;

namespace XML_Bit
{
    class Program
    {
        static void Main(string[] args)
        {
            
            List<machine> mm = new List<machine>();
            settings_json js = new settings_json();
            mm = js.loadsettings(mm);
          
            foreach(machine setting in mm)
            {
                Console.WriteLine("{3} Loaded settings: id:{0}, portd: {1}, counters: {2}", setting.id, setting.ports, setting.counters, logTime());
            }

            Console.WriteLine("SetPort: {0}", mm[0].SetPort(0, 1));
            mm[0].SetCounter(0, 23003);
            //mm[1].SetCounter(1, 550022);
            Console.WriteLine("machine id: {0}, ports: {1}", mm[0].id, mm[0].ports);
            Console.WriteLine("machine id: {0}, counter: {1}", mm[0].id, mm[0].GetCounter(1));
            mm[0].CountUp(1);
            Console.WriteLine("machine id: {0}, counter: {1}", mm[0].id, mm[0].GetCounter(1));

            Console.WriteLine("Antal maskiner {0}", mm.Count());

            xmlHandler xDoc = new xmlHandler();
            XmlDocument doc =  xDoc.CreateXml(mm);
            xDoc.saveAsFile();
            Console.Write(doc.OuterXml.ToString());

            
            /******/
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add("http://tempuri.org/MachineSchema.xsd", "Machine.xsd");
                settings.ValidationType = ValidationType.Schema;

                XmlReader reader = XmlReader.Create("Machine.xml", settings);
                XmlDocument document = new XmlDocument();
                document.Load(reader);

                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);

                // the following call to Validate succeeds.
                document.Validate(eventHandler);
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            Console.WriteLine("***********************************************");
            mm[0].SetCounter(0, 50001);
            mm[0].SetPort(0, 1);
            mm[0].SetCounter(1, 9999);
            mm[0].SetPort(1, 0);
            mm[0].SetCounter(2, 123456);
            mm[0].SetPort(2, 0);

            mm[1].SetCounter(0, 10000);
            mm[1].SetPort(0, 1);
            mm[1].SetCounter(1, 20000);
            mm[1].SetPort(1, 0);
            mm[1].SetCounter(2, 30000);
            mm[1].SetPort(2, 0);
            doc = xDoc.updateMachine(mm[0], doc);
            doc = xDoc.updateMachine(mm[1], doc);
            doc.Save("Machine2.xml");
            
            

            js.writesettings("setings.json", mm);
            
            Console.ReadLine();
        }

/* Validera xml:en */
        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Console.WriteLine("Error: {0}", e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Console.WriteLine("Warning {0}", e.Message);
                    break;
            }
        }

        static string logTime()
        {
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            return dt.ToString("yyyy-MM-dd HH:mm:ss.ms");
        }


    }
}
