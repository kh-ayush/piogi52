using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MainGame.Classes
{
    public class CObject
    {
        private System.Drawing.Point position;
        private System.Drawing.Size size;
        private double lifetime;
        private Ellipse sprite;

        public double Lifetime => lifetime;
        public Ellipse Sprite => sprite;

        public CObject(System.Drawing.Point position, double size, double lifetime)
        {
            this.position = position;
            this.size = new System.Drawing.Size((int)size, (int)size);
            this.lifetime = lifetime;

            sprite = new Ellipse()
            {
                Width = this.size.Width,
                Height = this.size.Height,
                Stroke = Brushes.White,
                StrokeThickness = 2,
                Fill = Brushes.Gray,

                RenderTransform = new TranslateTransform(position.X, position.Y)
            };
        }
        public Ellipse GetSprite() { return Sprite; }
        public bool isMouseOnObject(System.Drawing.Point mousePos)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(position, size);
            return rect.Contains(mousePos);
        }

        public bool updateLifetime(double delta)
        {
            lifetime -= delta;
            return lifetime > 0;
        }
    }
}