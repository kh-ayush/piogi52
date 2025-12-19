using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MainGame.Classes
{
    public abstract class CObject
    {
        private System.Drawing.Point position;
        private System.Drawing.Size size;
        private double lifetime;
        private Ellipse sprite;
        private string message;

        public double Lifetime => lifetime;
        public Ellipse Sprite => sprite;
        public string Message => message;

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

                RenderTransform = new TranslateTransform(position.X, position.Y)
            };
            sprite.Fill = GetFillBrush();
        }
        protected abstract Brush GetFillBrush();
        protected abstract void GetBonus(CPlayer player);
        public Ellipse GetSprite() { return Sprite; }
        public void ApplyBonus(CPlayer player)
        {
            GetBonus(player);
        }
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