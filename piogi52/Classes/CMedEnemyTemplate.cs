using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using piogi52.Interfaces;

namespace piogi52.Classes
{
    public class CMedEnemyTemplate : CEnemyTemplate, IMed
    {
        double heal;
        public double Heal
        {
            get { return heal; }
            set { if (value > 0) heal = value; else heal = 0.5; }
        }
    }
}
