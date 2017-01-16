using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_Bit
{
    class machine
    {
        public machine(string id = "127.0.0.1", int ports = 1, int counters = 1)
        {
            _mid = id;
            _setports(ports);
            _setcounters(counters);
            for(int i = 0; i < ports-1;i++)
                SetPort(i, 0);
            for (int i = 0; i < counters - 1; i++)
                SetCounter(i, 0);
        }

        public void Init(string id, int ports, int counters)
        {
            _mid = id;
            _setports(ports);
            _setcounters(counters);

        }
        // sätt en port           
        public bool SetPort(int port, int status)
        {
            if (port > ports || port < 0)
                return false;
            _lport[port] = status;
            return true;
        }
        //hämta en port
        public int GetPort(int port)
        {
            try
            {
                return _lport[port];
            }
            catch
            {
                return -1;
            }
        }
        //hämta alla portar
        public int[] getAllPorts()
        {
            int[] ig = new int[_lport.Count()];
            for (int i = 0; i < _lport.Count(); i++)
            {
                ig[i] = (_lport[i] == 1) ? 1 : 0;
            }
            return ig;
        }
        //lägg till portar
        private void _setports(int _ports)
        {
            _mport = _ports;
            for (int i = 0; i < _ports; i++)
            {
                _lport.Add(0);

            }
        }
        
        // hämta en räknare
        public int GetCounter(int counter)
        {
            return _lcounter[counter];
        }
        //räkna upp
        public void CountUp(int counter)
        {
            _lcounter[counter]++;
        }
        //nolla rn räknare
        public void ResetCounter(int counter)
        {
            _lcounter[counter] = 0;
        }
        //nolla alla räknare
        public void ResetAllCounters()
        {
            for (int i = 0; i < _lcounter.Count; i++)
                _lcounter[i] = 0;
        }
        //sätt en räknare
        public bool SetCounter(int counter, int value)
        {
            if (counter > counters || counter < 0)
                return false;
            _lcounter[counter] = value;
            return true;
        }
        // visa alla räknare
        public int[] getAllCounters()
        {
            int[] ic = new int[_lcounter.Count()];
            for (int i = 0; i < _lcounter.Count(); i++)
            {
                ic[i] = _lcounter[i];
            }
            return ic;
        }

        
        /* PRIVATE*/

        // lägg ill räknarna
        private void _setcounters(int _counters)
        {
            _mcounters = _counters;
            for (int i = 0; i < _counters; i++)
            {
                _lcounter.Add(0);
            }
        }

        //Standard constructor
        private string _mid = "127.0.0.1";
        private int _mport = 1;
        private int _mcounters = 1;
        //
        private List<int> _lport = new List<int>();
        private List<int> _lcounter = new List<int>();
        //
        public string id { get { return _mid; } set { _mid = id; } }
        public int ports { get { return _lport.Count; } set { _mport = ports; _setports(_mport); } }
        public int counters { get { return _lcounter.Count; } set { _mcounters = counters; _setcounters(_mcounters); } }


    }
}
