using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Diagnostics;

namespace XML_Bit
{
    class xmlHandler
    {
        public XmlDocument doc = new XmlDocument();

        public XmlDocument CreateXml(machine[] mm)
        {
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode rootNode = doc.CreateElement("root");
            doc.AppendChild(rootNode);

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
            return doc;
        }
        /*
 * Editera XML med ny data *
 */
        public XmlDocument updateMachine(machine m, XmlDocument doc)
        {            
            /*XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add("http://tempuri.org/MachineSchema.xsd", "Machine.xsd");
            settings.ValidationType = ValidationType.Schema;*/

            XPathNavigator navigator = doc.CreateNavigator();
            navigator.MoveToFirst();

            navigator.MoveToChild("root", String.Empty);// "http://tempuri.org/MachineSchema.xsd") == true)
            navigator.MoveToChild("source", String.Empty);// "http://tempuri.org/MachineSchema.xsd");

            do
            {
                if (navigator.HasAttributes)
                {
                    if (navigator.GetAttribute("id", String.Empty) == m.id)
                    {
                        XPathNavigator port = navigator.Clone();
                        port.MoveToChild("inputs", String.Empty); //"http://tempuri.org/MachineSchema.xsd");
                        port.MoveToChild("input", String.Empty); // "http://tempuri.org/MachineSchema.xsd");
                        Console.WriteLine(port.Value);

                        if (port.HasAttributes)
                        {
                            int iport = 1;
                            do
                            {
                                if (port.GetAttribute("port", String.Empty) == iport.ToString())
                                {
                                    XPathNavigator statusnav = port.Clone();
                                    statusnav.MoveToChild("status", String.Empty);
                                    statusnav.SetValue(m.GetPort(iport - 1).ToString());

                                    statusnav.MoveToNext("counter", string.Empty);
                                    statusnav.SetValue(m.GetCounter(iport - 1).ToString());
                                    iport++;
                                }
                            } while (port.MoveToNext("input", string.Empty));

                            navigator.MoveToFirstAttribute();
                        }
                    }
                }
            }
            while (navigator.MoveToNext("source", string.Empty));

            return doc;
        }

        public void saveAsFile(string filename = "Machine.xml")
        {
            doc.Save(@filename);
        }
        private XmlNode AddNewMachine(string id, int[] port, int[] counter, XmlDocument doc, XmlNode rootNode)
        {
            // lägg till maskin
            XmlNode sourceNode = doc.CreateElement("source");
            XmlAttribute sourceAttribute = doc.CreateAttribute("id");
            sourceAttribute.Value = id;
            sourceNode.Attributes.Append(sourceAttribute);
            rootNode.AppendChild(sourceNode);

            // lägg till portar
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
    }
}
