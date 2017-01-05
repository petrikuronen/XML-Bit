using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
        public XmlNode updateMachine(machine m, XmlDocument doc, XmlNode rootNode = null)
        {
            //XmlNodeList mNodes = doc.SelectNodes("/root/source[@id='"+ m.id+"']");
            XmlNode mId = doc.SelectSingleNode("/root/source[@id='" + m.id + "']");
            XmlNodeList mPorts = mId.SelectNodes("inputs/input");
            String tmp = "";
            foreach (XmlNode mPort in mPorts)
            {
                XmlAttributeCollection xa = mPort.Attributes;

                tmp = "Port: " + xa.Item(0).Value;
                int iPort = Int32.Parse(xa.Item(0).Value);
                 XmlNode oldPort = mPort.SelectSingleNode("status");
                 mPort.SelectSingleNode("status").InnerText = m.GetPort(iPort).ToString();
                tmp += ", Counter: " + mPort.SelectSingleNode("counter").InnerText;
                Console.WriteLine(tmp);
                doc.ReplaceChild(mPort, oldPort);
            }

            // save the XmlDocument back to disk
            //doc.Save(@"Machine.xml");

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
