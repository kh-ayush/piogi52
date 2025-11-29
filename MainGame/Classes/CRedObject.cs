using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
namespace MainGame.Classes
{
    public class CRedObject : CObject
    {
        public CRedObject(System.Drawing.Point position, double size, double lifetime)
            : base(position, size, lifetime)
        {
            Sprite.Fill = Brushes.Red;
            Sprite.Stroke = Brushes.DarkRed;
            Sprite.StrokeThickness = 1;
        }
    }
}
