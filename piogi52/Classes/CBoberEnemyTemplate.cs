using piogi52.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace piogi52.Classes
{
    public class CBoberEnemyTemplate : CEnemyTemplate, IBober
    {
        double bober;
        [JsonInclude]
        public double Bober
        {
            get { return bober; }
            set { //if (value > 0) bober = value; 
                //else 
                    bober = 0.5; }
        }
    }
}
