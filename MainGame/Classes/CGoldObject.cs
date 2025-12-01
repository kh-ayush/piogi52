using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MainGame.Classes
{
    public class CGoldObject : CObject
    {
        public CGoldObject(System.Drawing.Point position, double size, double lifetime)
            : base(position, size, lifetime)
        { }

        protected override Brush GetFillBrush()
        {
            return Brushes.Orange;
        }
        protected override void GetBonus(CPlayer player)
        {

        }
    }
}
