using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XML_Bit
{
    class Program
    {
        static void Main(string[] args)
        {
            machine[] mm = new machine[5];//("192.168.0.1", 10, 10);
            for(int i = 0; i < 5; i++)
                mm[i] = new machine("192.168.0.11"+i.ToString(), 3, 3);
            //mm[1] = new machine("192.168.0.22", 3, 3);
            Console.WriteLine(mm[0].SetPort(1, true));
            mm[0].SetCounter(0, 23003);
            mm[1].SetCounter(1, 550022);
            Console.WriteLine("machine id: {0}, ports: {1}", mm[0].id, mm[0].ports);
            Console.WriteLine("machine id: {0}, counter: {1}", mm[0].id, mm[0].GetCounter(1));
            mm[0].CountUp(1);
            Console.WriteLine("machine id: {0}, counter: {1}", mm[0].id, mm[0].GetCounter(1));

            Console.WriteLine(mm[0].getAllPorts());
            //mm[0].ResetAllCounters();
            Console.WriteLine(mm[0].getAllCounters());
            Console.WriteLine("Antal maskiner {0}", mm.Count());

            XmlDocument doc = new XmlDocument();
            //XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement ele = doc.CreateElement("root"); ;
                              
            
            // Create a new element.  
            for(int i = 0; i < mm.Length; i++)
                ele.AppendChild(AddNewMachine(mm[i].id, mm[i].getAllPorts(), mm[i].getAllCounters(), doc));
            
            Console.Write(ele.OuterXml.ToString());
            Console.ReadLine();
        }


       static XmlElement AddNewMachine(string id, int[] port, int[] counter, XmlDocument doc)
        {
            XmlElement machineElement = doc.CreateElement("source");
            
            XmlAttribute attribute = doc.CreateAttribute("id");
            attribute.Value = id;
            
            machineElement.Attributes.Append(attribute);

            XmlElement portsElement = doc.CreateElement("inputs");
            machineElement.AppendChild(portsElement);
            
            // lägg till status på portarna
            for (int i = 0, j = 1; i < port.Length; i++, j++)
            {
                XmlNode portElement = doc.CreateElement("input");
                XmlAttribute portAttribute = doc.CreateAttribute("port");
                portAttribute.Value = j.ToString();

                XmlElement status = doc.CreateElement("status");
                portElement.AppendChild(status);
                status.InnerText = port[i].ToString();
                
                //räknare
               
                XmlElement count = doc.CreateElement("counter");
                count.InnerText = counter[i].ToString();
                status.AppendChild(count);

                portElement.Attributes.Append(portAttribute);
                portsElement.AppendChild(portElement);
               
            }
            machineElement.AppendChild(portsElement);


            return machineElement;// rootElement;
            ;

        }
    }
}
