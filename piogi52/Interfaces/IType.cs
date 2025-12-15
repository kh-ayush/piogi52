using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace piogi52.Interfaces
{
    public interface IArmor
    {
        double Armor { get; set; }
    }
    public interface IMed
    {
        double Heal { get; set; }
    }
    public interface IBober
    {
        double Bober { get; set; }
    }
}
