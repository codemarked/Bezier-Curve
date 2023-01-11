using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;

namespace Grafica1
{
    public class Main
    {
        private static Main INSTANCE;

        public static Main Get() { return INSTANCE; }

        public static readonly Color BACKGROUND = Color.DarkSlateGray;

        public PictureBox pictureBox;
        public Graphics graphics;
        public Bitmap bitMap;

        public static bool isDrawingDebugLines = false;
        public static bool isDrawingLowerOrderLines = true;

        public int width, height;
        public int BoundX, BoundY;

        public int MARGIN = 10;

        public static readonly Point origin = new Point(0, 0, Color.Gold, 15);
        public List<Point> points = new List<Point>();

        public Main(PictureBox box)
        {
            INSTANCE = this;
            pictureBox = box;
            width = box.Width;
            height = box.Height;
            bitMap = new Bitmap(width, height);
            graphics = Graphics.FromImage(bitMap);
            BoundX = width / 2;
            BoundY = height / 2;
            BoundX -= 7;
            BoundY -= 7;
            Init();
            RefreshGraph();
        }
        public void Init()
        {
            ClearGraph();
            DrawLine(-BoundX, 0, BoundX - 10, 0, Color.LightGray, 5);
            DrawLine(0, -BoundY, 0, BoundY - 10, Color.LightGray, 5);
            DrawPoint(origin);
            DrawString(-20,-20, "o", Color.LightGray, 20);
            DrawArrow(0, BoundY - 3, 0, Color.LightGray);
            DrawString(-30, BoundY - 10, "y", Color.LightGray, 20);
            DrawArrow(BoundX - 3, 0, 1, Color.LightGray);
            DrawString(BoundX - 10, -30, "x", Color.LightGray, 20);
        }

        public void ClearAll()
        {
            points.Clear();
            time = 0.0f;
            Init();
            RefreshGraph();
        }

        public float TX(float x)
        {
            return Math.Max(Math.Min(BoundX + x, width - MARGIN), MARGIN);
        }
        public float TY(float y)
        {
            return Math.Max(Math.Min(BoundY - y, height - MARGIN), MARGIN);
        }

        public float DX(float x)
        {
            return Math.Min(Math.Max(x - BoundX, MARGIN - width), width - MARGIN);
        }
        public float DY(float y)
        {
            return Math.Min(Math.Max(BoundY - y, MARGIN - height), height - MARGIN);
        }

        public void DrawPoints()
        {
            foreach (Point p in points)
            {
                DrawPoint(p);
                DrawCoords(p);
            }
            DrawPoint(origin);
        }

        public void DrawPoint(Point point)
        {
            DrawPoint((int)point.X, (int)point.Y, point.getColor(), point.getSize());
        }
        public void DrawPoint(PointF point, Color color, int size)
        {
            DrawPoint((int)point.X, (int)point.Y, color, size);
        }
        public void DrawPoint(int x, int y, Color color, int size)
        {
            graphics.FillEllipse(new SolidBrush(color), TX(x - size / 2), TY(y + size / 2), size, size);
        }
        public void DrawLine(PointF a, PointF b, Color color, int size)
        {
            DrawLine((int)a.X, (int)a.Y, (int)b.X, (int)b.Y, color, size);
        }
        public void DrawLine(int x1, int y1, int x2, int y2, Color color, int size)
        {
            graphics.DrawLine(new Pen(color, size), TX(x1), TY(y1), TX(x2), TY(y2));
        }

        public void DrawArrow(PointF a, int direction, Color color) 
        {
            DrawArrow(a, direction, color);
        }
        public void DrawArrow(int x, int y, int direction, Color color)
        {
            if (direction == 0)
            {
                graphics.FillPolygon(new SolidBrush(color), new PointF[] { new PointF(TX(x), TY(y) - 7), new PointF(TX(x - 10), TY(y - 40)), new PointF(TX(x + 10), TY(y - 40)) });
            }
            else if (direction == 1)
            {
                graphics.FillPolygon(new SolidBrush(color), new PointF[] { new PointF(TX(x - 40), TY(y + 10)), new PointF(TX(x - 40), TY(y - 10)), new PointF(TX(x) + 7, TY(y)) });
            }
            else if (direction == 2)
            {
                DrawLine(x, y, x - 5, y + 5, color, 6);
                DrawLine(x, y, x + 5, y + 5, color, 6);
            }
            else if (direction == 3)
            {
                DrawLine(x, y, x + 5, y + 5, color, 6);
                DrawLine(x, y, x + 5, y - 5, color, 6);
            }
        }

        public void DrawCoords(Point p)
        {
            DrawString(new PointF(p.X - p.getSize() * 2,p.Y + p.getSize() * 2), $"{p.getName()} ({(int)p.X},{(int)p.Y})", Color.White, p.getSize());
        }
        public void DrawString(PointF a, string str, Color color, int size)
        {
            DrawString((int)a.X, (int)a.Y, str, color, size);
        }
        public void DrawString(int x, int y, string str, Color color, int size)
        {
            graphics.DrawString(str, new Font("Arial", size, FontStyle.Regular), new SolidBrush(color), TX(x) - size, TY(y) - size);
        }

        public void RefreshGraph()
        {
            pictureBox.Image = bitMap;
        }

        public void ClearGraph()
        {
            graphics.Clear(BACKGROUND);
        }

        public static readonly float delta = 0.003f;
        public static readonly float perTick = 0.05f;

        public static float time = 0.00f;

        public void Run()
        {
            if (time >= 1.0f)
            {
                return;
            }
            List<Line> lines = new List<Line>();
            for (float i = 0.0f; i <= perTick && time <= 1.0f; i += delta) {
                if (isDrawingDebugLines)
                    lines = Bezzier(time = Math.Min(1.0f, time + delta));
                else
                    Bezzier(time = Math.Min(1.0f, time + delta));
            }
            if (isDrawingDebugLines && lines != null && lines.Count > 1)
            {
                int i = 0;
                foreach (Line line in lines)
                {
                    DrawLine(line.p1, line.p2, Utils.getColor(i), 1);
                    i++;
                }
            }
            DrawPoints();
            RefreshGraph();
        }
        public void Run(float t)
        {
            Init();
            Bezzier(t);
            DrawPoints();
            RefreshGraph();
        }

        public void Apply()
        {
            Init();
            List<Line> lines = new List<Line>();
            for (float i = 0.0f; i <= time; i += delta)
            {
                if (isDrawingDebugLines)
                    lines = Bezzier(i);
                else
                    Bezzier(i);
            }
            if (isDrawingDebugLines && lines != null && lines.Count > 1)
            {
                int i = 0;
                foreach (Line line in lines)
                {
                    DrawLine(line.p1, line.p2, Utils.getColor(i), 1);
                    i++;
                }
            }
            DrawPoints();
            RefreshGraph();
        }

        public List<Line> Bezzier(float t)
        {
            List<Line> list = new List<Line>();
            if (points.Count < 2)
                return list;
            PointF Q0 = FunctionBezzier(t, points[0].ToPointF(), points[1].ToPointF()); // Linear
            if (isDrawingLowerOrderLines)
                DrawPoint(Q0, Utils.getColor(0), 4);
            if (points.Count < 3)
                return list;
            PointF Q1 = FunctionBezzier(t, points[1].ToPointF(), points[2].ToPointF());
            if (isDrawingLowerOrderLines)
                DrawPoint(Q1, Utils.getColor(1), 4);
            if (isDrawingDebugLines)
                list.Add(new Line(Q0, Q1));
            PointF R0 = FunctionBezzier(t, Q0, Q1); // Cuadratic
            if (isDrawingLowerOrderLines)
                DrawPoint(R0, Utils.getColor(2), 4);
            if (points.Count < 4)
                return list;
            PointF Q2 = FunctionBezzier(t, points[2].ToPointF(), points[3].ToPointF());
            if (isDrawingLowerOrderLines)
                DrawPoint(Q2, Utils.getColor(3), 4);
            if (isDrawingDebugLines)
                list.Add(new Line(Q1, Q2));
            PointF R1 = FunctionBezzier(t, Q1, Q2);
            if (isDrawingLowerOrderLines)
                DrawPoint(R1, Utils.getColor(4), 4);
            if (isDrawingDebugLines)
                list.Add(new Line(R0, R1));
            PointF S0 = FunctionBezzier(t, R0, R1); // Cubic
            DrawPoint(S0, Utils.getColor(5), 6);
            return list;
        }

        public PointF FunctionBezzier(float t, PointF p0, PointF p1)
        {
            return new PointF((1.0f - t) * p0.X + t * p1.X, (1.0f - t) * p0.Y + t * p1.Y);
        }

/*        public Point Bezzier(int start, int end, float t)
        {
            if (start == end) 
                return points[0];
            Point p0 = Bezzier(start, end - 1, t);
            Point p1 = Bezzier(start + 1, end, t);
            float delta = 1 - t;
            DrawPoint(p0);
            DrawPoint(p1);
            if (isDrawingDebugLines)
                DrawLine(p0.ToPointF(), p1.ToPointF(), Utils.getColor(-1), 5);
            Point p = new Point(delta * p0.X + t * p1.X, delta * p0.Y + t * p1.Y, Utils.getColor(-1), 5);
            DrawPoint(p);
            return p;
        }*/
    }
}
