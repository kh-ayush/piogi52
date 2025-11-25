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
        private double pointsValue;
        private Ellipse sprite;

        public double Lifetime => lifetime;
        public Ellipse Sprite => sprite;

        public CObject(System.Drawing.Point position, double size, double lifetime)
        {
            this.position = position;
            this.size = new System.Drawing.Size((int)size, (int)size);
            this.lifetime = lifetime;

            sprite = new Ellipse();
            sprite.Width = this.size.Width;
            sprite.Height = this.size.Height;

            sprite.Fill = Brushes.Gray ;
            sprite.Stroke = Brushes.White;
            sprite.StrokeThickness = 2;

            sprite.RenderTransform = new TranslateTransform(position.X, position.Y);

            //pointsValue = ((1 / this.size.Width) / lifetime) * 1000;
            pointsValue = 1;
        }

        public bool isMouseOnObject(System.Drawing.Point mousePos)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(position, size);
            return rect.Contains(mousePos);
        }

        public double getPointsValue() => pointsValue;

        public bool updateLifetime(double delta)
        {
            lifetime -= delta;
            return lifetime > 0;
        }
    }
}