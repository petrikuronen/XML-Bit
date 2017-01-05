using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_Bit
{
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
            for (int i = 0; i < _lcounter.Count(); i++)
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
        public int ports { get { return _lport.Count; } set { _mport = ports; _setports(_mport); } }
        public int counters { get { return _lcounter.Count; } set { _mcounters = counters; _setcounters(_mcounters); } }


    }
}
