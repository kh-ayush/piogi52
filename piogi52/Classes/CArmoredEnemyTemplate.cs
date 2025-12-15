using piogi52.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace piogi52.Classes
{
    public class CArmoredEnemyTemplate : CEnemyTemplate, IArmor
    {
        double armor;
        [JsonInclude]
        public double Armor
        {
            get { return armor; }
            set { //if (value > 0) armor = value; 
                //else 
                    armor = 25; }
        }
    }
}
