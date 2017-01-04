using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace XML_Bit
{
    class Program
    {
        static void Main(string[] args)
        {
            machine[] mm = new machine[1];//("192.168.0.1", 10, 10);
            for(int i = 0; i < 1; i++)
                mm[i] = new machine("192.168.0.11"+i.ToString(), 3, 3);
            //mm[1] = new machine("192.168.0.22", 3, 3);
            Console.WriteLine(mm[0].SetPort(1, true));
            mm[0].SetCounter(0, 23003);
            //mm[1].SetCounter(1, 550022);
            Console.WriteLine("machine id: {0}, ports: {1}", mm[0].id, mm[0].ports);
            Console.WriteLine("machine id: {0}, counter: {1}", mm[0].id, mm[0].GetCounter(1));
            mm[0].CountUp(1);
            Console.WriteLine("machine id: {0}, counter: {1}", mm[0].id, mm[0].GetCounter(1));

            Console.WriteLine(mm[0].getAllPorts());
            //mm[0].ResetAllCounters();
            Console.WriteLine(mm[0].getAllCounters());
            Console.WriteLine("Antal maskiner {0}", mm.Count());

            XmlDocument doc = new XmlDocument();
            XmlDocument doc1 = new XmlDocument();
            /**/
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode rootNode = doc.CreateElement("root");
            doc.AppendChild(rootNode);
            /**/


            // Create a new element.  
            for (int i = 0; i < mm.Length; i++)
                AddNewMachine(mm[i].id, mm[i].getAllPorts(), mm[i].getAllCounters(), doc, rootNode);

             XmlWriterSettings setting = new XmlWriterSettings();
             setting.Indent = true;
             setting.IndentChars = ("\t");
             setting.OmitXmlDeclaration = true;
            setting.ConformanceLevel = ConformanceLevel.Fragment;

             // Create the XmlWriter object and write some content.
             XmlWriter write = null;
             write = XmlWriter.Create("Machine.xml", setting);
            
            doc.WriteContentTo(write);
            write.Close();

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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Häpp");
                Console.WriteLine(ex.Message);
                
            }
        

       
    
    /******/
    Console.ReadLine();
        }


       static XmlNode AddNewMachine(string id, int[] port, int[] counter, XmlDocument doc, XmlNode rootNode) //XmlElement
        {
            /****/
            XmlNode sourceNode = doc.CreateElement("source");
            XmlAttribute sourceAttribute = doc.CreateAttribute("id");
            sourceAttribute.Value = id;
            sourceNode.Attributes.Append(sourceAttribute);
            rootNode.AppendChild(sourceNode);

            XmlNode inputsNode = doc.CreateElement("inputs");
            sourceNode.AppendChild(inputsNode);

            // lägg till status på portarna
            for (int i = 0, j = 1; i < port.Length; i++, j++)
            {
                XmlNode inputNode = doc.CreateElement("input");
                XmlAttribute portAttribute = doc.CreateAttribute("port");
                portAttribute.Value = j.ToString();

                XmlElement status = doc.CreateElement("status");
                inputNode.AppendChild(status);
                status.InnerText = port[i].ToString();
                
                //räknare
                XmlElement count = doc.CreateElement("counter");
                count.InnerText = counter[i].ToString();
                inputNode.AppendChild(count);

                inputNode.Attributes.Append(portAttribute);
                inputsNode.AppendChild(inputNode);
               
            }
            sourceNode.AppendChild(inputsNode);
            return doc;
        }
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
    }
}
