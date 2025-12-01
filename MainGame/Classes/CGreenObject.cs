using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MainGame.Classes
{
    public class CGreenObject : CObject
    {
        public CGreenObject(System.Drawing.Point position, double size, double lifetime)
            : base(position, size, lifetime)
        { }

        protected override Brush GetFillBrush()
        {
            return Brushes.Green;
        }
        protected override void GetBonus(CPlayer player)
        {
            player.AddBonus(
                new PlayerBonus(
                    duration: 10,
                    apply: p =>
                    {
                        p.CoolDown /= 2;
                        p.SetCD();
                    },
                    remove: p => 
                    {
                        p.CoolDown *= 2;
                        p.SetCD();
                    }
                )
            );
        }
    }
}
