using MainGame.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MainGame.Classes
{
    public class CController
    {
        private List<CObject> objects;
        private Random rng;

        private double spawnRate;
        private double time;
        private double points;

        private double minLifetime = 1;
        private double maxLifetime = 5;
        private double minSpriteSize = 20;
        private double maxSpriteSize = 40;

        private Size sceneSize;

        public List<CObject> Objects => objects;
        public double Points => points;

        public CController(double spawnRate, double startTime, Size sceneSize)
        {
            rng = new Random();
            objects = new List<CObject>();

            this.spawnRate = spawnRate;
            this.sceneSize = sceneSize;

            time = startTime;
            points = 0;
        }

        public void spawnObject()
        {
            double lifetime = rng.NextDouble() * (maxLifetime - minLifetime) + minLifetime;
            double size = rng.NextDouble() * (maxSpriteSize - minSpriteSize) + minSpriteSize;

            int x = rng.Next(0, sceneSize.Width - (int)size);
            int y = rng.Next(0, sceneSize.Height - (int)size);

            int rnd = rng.Next(0, 3);
            CObject obj;

            switch (rnd)
            {
                case 0: obj = new CRedObject(new Point(x, y), size, lifetime); break;
                case 1: obj = new CGoldObject(new Point(x, y), size, lifetime); break;
                default: obj = new CGreenObject(new Point(x, y), size, lifetime); break;
            }

            objects.Add(obj);
        }

        public void destroyObject(CObject obj)
        {
            objects.Remove(obj);
        }

        public void update(double delta)
        {
            time += delta;

            if (time >= spawnRate)
            {
                spawnObject();
                time = 0;
            }

            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (!objects[i].updateLifetime(delta))
                    destroyObject(objects[i]);
            }
        }

        public CObject mouseClick(Point mousePos)
        {
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].isMouseOnObject(mousePos))
                {
                    CObject CurObj = objects[i];
                    destroyObject(CurObj);
                    return CurObj;
                }
            }
            return null;
        }
    }
}