using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafica1
{
    public class Line
    {
        public PointF p1, p2;

        public Line(PointF p1, PointF p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
    }
    public class Point
    {
        public static int COUNT = 0;
        private string name;
        public float X, Y;
        private int size = 5;
        private Color color = Color.White;
        public Point(PointF p)
        {
            this.X = p.X;
            this.Y = p.Y;
        }
        public Point(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point(float x, float y, Color color, int size)
        {
            this.X = x;
            this.Y = y;
            this.color = color;
            this.size = size;
            this.name = "P" + COUNT;
            COUNT++;
        }

        public bool isInside(Point p1, Point p2)
        {
            float minX = Math.Min(p1.X, p2.X);
            float minY = Math.Min(p1.Y, p2.Y);
            float maxX = Math.Max(p1.X, p2.X);
            float maxY = Math.Max(p1.Y, p2.Y);
            return X >= minX && X <= maxX && Y >= minY && Y <= maxY;
        }

        public string getName()
        {
            return name;
        }

        public PointF ToPointF()
        {
            return new PointF(X, Y);
        }

        public Color getColor()
        {
            return color;
        }

        public int getSize()
        {
            return size;
        }
    }
}
