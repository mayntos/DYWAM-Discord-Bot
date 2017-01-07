using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DYWAMBot
{
    class chatter
    {
        private int wallet;
        private int wins;
        private string name = "";

        public chatter(string n)
        {
            wallet = 100;
            wins = 0;
            name = n;
        }

        public string getName()
        {
            return name;
        }

        public void withdraw(int n)
        {
            wallet -= n;
        }
    }
}
