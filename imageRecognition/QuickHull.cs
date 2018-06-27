using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace imageRecognition
{
    class QuickHull
    {
        public void DrawHull(List<Point> Points, Graphics gr)
        {
            Pen pen = new Pen(Color.Red, 3);
            for (int i = 0; i < Points.Count - 1; i++)
            {
                gr.DrawLine(pen, Points[i], Points[i + 1]);
            }
            gr.DrawLine(pen, Points.Last(), Points[0]);
        }

        public List<Point> DeleteAnglePoints(List<Point> pt)
        {
            int k = 1;
//int i = 0;
            while (pt[k] != pt.Last<Point>())
            {
                bool m1 = (Math.Abs(Convert.ToInt32(pt[k + 1].X - pt[k - 1].X)) == 1) && (Math.Abs(Convert.ToInt32(pt[k + 1].Y - pt[k - 1].Y)) == 1);
                bool m2 = (pt[k + 1].X - pt[k - 1].X == 0) && (pt[k + 1].Y - pt[k - 1].Y == 0);
                bool m3 = (Math.Abs(Convert.ToInt32(pt[k + 1].X - pt[k - 1].X)) == 1) && (pt[k + 1].Y - pt[k - 1].Y == 0);
                if (m1 || m2 || m3)
                    pt.RemoveAt(k);
                else
                    k++;
            }
            //проверка первой точки
            bool n1 = (Math.Abs(Convert.ToInt32(pt[1].X - pt.Last<Point>().X)) == 1) && (Math.Abs(Convert.ToInt32(pt[1].Y - pt.Last<Point>().Y)) == 1);
            bool n2 = (pt[1].X - pt.Last<Point>().X == 0) && (pt[1].Y - pt.Last<Point>().Y == 0);
            bool n3 = (Math.Abs(Convert.ToInt32(pt[1].X - pt.Last<Point>().X)) == 1) && (pt[1].Y - pt.Last<Point>().Y == 0);
            if (n1 || n2 || n3)
                pt.RemoveAt(0);
            n1 = false;
            n2 = false;
            n3 = false;
            //проверка последней точки
            k = pt.Count - 1;
            n1 = (Math.Abs(Convert.ToInt32(pt[0].X - pt[k - 1].X)) == 1) && (Math.Abs(Convert.ToInt32(pt[0].Y - pt[k - 1].Y)) == 1);
            n2 = (pt[0].X - pt[k - 1].X == 0) && (pt[0].Y - pt[k - 1].Y == 0);
            n3 = (Math.Abs(Convert.ToInt32(pt[0].X - pt[k - 1].X)) == 1) && (pt[0].Y - pt[k - 1].Y == 0);
            if (n1 || n2 || n3)
                pt.RemoveAt(k);
            return pt;
        }

        public List<Point> quickHull(List<Point> points)
        {
            List<Point> convexHull = new List<Point>();
            if (points.Count < 3)
                return points;

            int minPoint = -1, maxPoint = -1;
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            //находим две самые крайние точки по оси x
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X < minX)
                {
                    minX = points[i].X;
                    minPoint = i;
                }
                if (points[i].X > maxX)
                {
                    maxX = points[i].X;
                    maxPoint = i;
                }
            }
            //записываем две найденные точки в А(самая левая) и В(самая правая)
            Point A = points[minPoint];
            Point B = points[maxPoint];
            //записываем их в результирующий набор
            convexHull.Add(A);
            convexHull.Add(B);
            //и удаляем из начального
            points.Remove(A);
            points.Remove(B);

            List<Point> leftSet = new List<Point>();
            List<Point> rightSet = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {
                Point p = points[i];
                //если точка выше прямой АВ
                if (pointLocation(A, B, p) == -1)
                    leftSet.Add(p);
                //если точка ниже прямой
                else if (pointLocation(A, B, p) == 1)
                    rightSet.Add(p);
            }
            hullSet(A, B, rightSet, convexHull);
            hullSet(B, A, leftSet, convexHull);
            List<Point> ostPoint = new List<Point>();
            for (int i = 0; i <= convexHull.Count - 1; i++)
            {
                if (i == convexHull.Count - 1)
                    Bresenham4Line(convexHull[i].X, convexHull[i].Y, convexHull[0].X, convexHull[0].Y, ostPoint);

                else
                    Bresenham4Line(convexHull[i].X, convexHull[i].Y, convexHull[i + 1].X, convexHull[i + 1].Y, ostPoint);
            }
            convexHull.AddRange(ostPoint);
            return convexHull;
        }

        public int GetDistance(Point A, Point B, Point C)
        {
            int ABx = B.X - A.X;
            int ABy = B.Y - A.Y;
            int num = ABx * (A.Y - C.Y) - ABy * (A.X - C.X);
            if (num < 0)
                num = -num;
            return num;
        }

        public void hullSet(Point A, Point B, List<Point> set, List<Point> hull)
        {
            //запоминаем индекс В выходном списке
            int insertPosition = hull.IndexOf(B);
            if (set.Count == 0)
                return;
            if (set.Count == 1)
            {
                Point p = set[0];
                //удаляем из входого списка
                set.Remove(p);
                //добавляем в выходной список 
                hull.Insert(insertPosition, p);
                return;
            }
            int dist = int.MinValue;
            int furthestPoint = -1;
            //проходим по всему входному списку
            for (int i = 0; i < set.Count; i++)
            {
                Point p = set[i];
                //рассчитываем расстояние до прямой
                int distance = GetDistance(A, B, p);
                //если эта точка самая удаленная от прямой то запоминаем
                if (distance > dist)
                {
                    dist = distance;
                    furthestPoint = i;
                }
            }
            //удаляем найденную точку из входного списка
            Point P = set[furthestPoint];
            set.RemoveAt(furthestPoint);
            //и записываем в выходной
            hull.Insert(insertPosition, P);
            //формируем список для прямых АР
            List<Point> leftSetAP = new List<Point>();
            for (int i = 0; i < set.Count; i++)
            {
                Point M = set[i];
                if (pointLocation(A, P, M) == 1)
                {
                    leftSetAP.Add(M);
                }
            }

            //и PB
            List<Point> leftSetPB = new List<Point>();
            for (int i = 0; i < set.Count; i++)
            {
                Point M = set[i];
                if (pointLocation(P, B, M) == 1)
                {
                    leftSetPB.Add(M);
                }
            }
            //повторяе функцию для этих прямых
            hullSet(A, P, leftSetAP, hull);
            hullSet(P, B, leftSetPB, hull);
        }

        public int pointLocation(Point A, Point B, Point P)
        {
            int cp1 = (B.X - A.X) * (P.Y - A.Y) - (B.Y - A.Y) * (P.X - A.X);
            if (cp1 > 0)
                return 1;
            else if (cp1 == 0)
                return 0;
            else
                return -1;
        }
        public void Bresenham4Line(int x0, int y0, int x1, int y1, List<Point> rez)
        {
            //Изменения координат
            int dx = (x1 > x0) ? (x1 - x0) : (x0 - x1);
            int dy = (y1 > y0) ? (y1 - y0) : (y0 - y1);
            //Направление приращения
            int sx = (x1 >= x0) ? (1) : (-1);
            int sy = (y1 >= y0) ? (1) : (-1);

            if (dy < dx)
            {
                int d = (dy << 1) - dx;
                int d1 = dy << 1;
                int d2 = (dy - dx) << 1;
                int x = x0 + sx;
                int y = y0;
                for (int i = 1; i <= dx; i++)
                {
                    if (d > 0)
                    {
                        d += d2;
                        y += sy;
                    }
                    else
                        d += d1;
                    Point p = new Point(x, y);
                    rez.Add(p);

                    x += sx;
                }
            }
            else
            {
                int d = (dx << 1) - dy;
                int d1 = dx << 1;
                int d2 = (dx - dy) << 1;
                int x = x0;
                int y = y0 + sy;
                for (int i = 1; i <= dy; i++)
                {
                    if (d > 0)
                    {
                        d += d2;
                        x += sx;
                    }
                    else
                        d += d1;
                    Point p = new Point(x, y);
                    rez.Add(p);
                    y += sy;
                }
            }
        }
        public void PutPixel(Graphics g, Color col, int x, int y, int alpha)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, col)), x, y, 3, 3);
        }
    }
}
