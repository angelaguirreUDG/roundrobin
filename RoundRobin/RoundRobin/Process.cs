using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoundRobin
{
    class Process
    {
        private string _name;
        private int _quantum;
        private int _burst;
        private string _status;

        public Process(string name, int quantum, int burst)
        {
            _name    = name;
            _quantum = quantum;
            _burst   = burst;
        }

        public int Quantum { get => _quantum; set => _quantum = value; }
        public string Name { get => _name; set => _name = value; }

        public int Burst { get => _burst; set => _burst = value; }

        public string Status { get => _status; set => _status = value; }

    }
}
