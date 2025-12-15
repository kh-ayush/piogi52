using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using piogi52.Interfaces;

namespace piogi52.Classes
{
    public class CBoberEnemyTemplate : CEnemyTemplate, IBober
    {
        double bober;
        public double Bober
        {
            get { return bober; }
            set { if (value > 0) bober = value; else bober = 25; }
        }
    }
}
