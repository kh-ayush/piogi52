using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using piogi52.Interfaces;

namespace piogi52.Classes
{
    public class CArmoredEnemyTemplate : CEnemyTemplate, IArmor
    {
        double armor;
        public double Armor
        {
            get { return armor; }
            set { if (value > 0) armor = value; else armor = 25; }
        }
    }
}
