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
            mm[0] = new machine("192.168.0.11", 3, 3);
            mm[1] = new machine("192.168.0.22", 3, 3);
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
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement ele = doc.CreateElement("root"); ;
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);
                  
            
            // Create a new book element.            
            ele.AppendChild(AddNewMachine(mm[0].id, mm[0].getAllPorts(), mm[0].getAllCounters(),doc));
            ele.AppendChild(AddNewMachine(mm[1].id, mm[1].getAllPorts(), mm[1].getAllCounters(), doc));
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



        class machine
        {
            public machine(string _id, int _ports, int _counters)
            {
                _mid = _id;
                _setports(_ports);
                _setcounters(_counters);   
            }
            // sätt en port           
            public bool SetPort(int _port, bool _status)
            {
                if (_port > ports || _port < 0)
                    return false;
                _lport[_port] = _status;
                    return true;
            }
            //hämta en port
            public bool GetPort(int _port)
            {
                    return _lport[_port];
            }
            //hämta alla portar
            public int[] getAllPorts()
            {
                int[] ig = new int[_lport.Count()];
                for (int i = 0; i < _lport.Count(); i++)
                {
                    ig[i] = (_lport[i] == true) ? 1 : 0;
                }
                return ig;
                //return string.Join("", _lport.ToArray());
            }
            //lägg till portar
            private void _setports(int _ports)
            {
                _mport = _ports;
                for (int i = 0; i < _ports; i++)
                {
                    _lport.Add(false);

                }
            }
            // lägg ill räknarna
            private void _setcounters(int _counters)
            {
                _mcounters = _counters;
                for (int i = 0; i < _counters; i++)
                {
                    _lcounter.Add(0);
                }
            }
            // hämta en räknare
            public int GetCounter(int _counter)
            {
                return _lcounter[_counter];
            }
            //räkna upp
            public void CountUp(int _counter)
            {
                _lcounter[_counter]++;
            }
            //nolla rn räknare
            public void ResetCounter(int _counter)
            {
                _lcounter[_counter] = 0;
            }
            //nolla alla räknare
            public void ResetAllCounters()
            {
                for (int i = 0; i < _lcounter.Count; i++)
                    _lcounter[i] = 0;
            }
            //sätt en räknare
            public bool SetCounter(int _counter, int _value)
            {
                if (_counter > counters || _counter < 0)
                    return false;
                _lcounter[_counter] = _value;
                return true;
            }
            // visa alla räknare
            public int[] getAllCounters()
            {
                int[] ic = new int[_lcounter.Count()];
                for(int i = 0; i < _lcounter.Count(); i++)
                {
                    ic[i] = _lcounter[i];
                }
                return ic;
            }
            //Standard constructor
            private string _mid = "127.0.0.1";
            private int _mport = 1;
            private int _mcounters = 1;
            //
            private List<bool> _lport = new List<bool>();
            private List<int> _lcounter = new List<int>();
            //
            public string id { get { return _mid; } set { _mid = id; } }
            public int ports { get { return _lport.Count; } set { _mport = ports;  _setports(_mport); } }
            public int counters { get { return _lcounter.Count; } set { _mcounters = counters;  _setcounters(_mcounters); } }

           
        }
    }
}
