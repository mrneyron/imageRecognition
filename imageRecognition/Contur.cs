using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace imageRecognition
{
    class Contur
    {
        private int i = 0, j = 0, n = 0;
        private int prev = 0;


        public void PutPixel(Graphics g, Color col, int x, int y, int alpha)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, col)), x, y, 3, 3);
        }

        public List<Point> DeleteAnglePoints(List<Point> pt)
        {
            //try
            //{
                int k = 1;
                while (k < pt.Count - 1)
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
            //}
            //catch { }
            return pt;
                
        }


        public List<Point> kontur(Bitmap img)
        {
            List<Point> ress = new List<Point>();
            //try
            //{
                // Find 1st black point
                bool f = false;
                for (i = 0; i < img.Width - 1; i++)
                {
                    for (j = 0; j < img.Height - 1; j++)
                    {
                        if (img.GetPixel(i, j).R == 255)
                        {
                            ress.Add(new Point(i, j));
                            f = true;
                            break;
                        }
                    }
                    if (f) break;
                }

                //Find 2nd black point

                if (img.GetPixel(i, j + 1).R == 255)
                {
                    ress.Add(new Point(i, j + 1));
                    prev = 1;
                }
                else if (img.GetPixel(i + 1, j + 1).R == 255)
                {
                    ress.Add(new Point(i + 1, j + 1));

                    prev = 2;
                }
                else if (img.GetPixel(i + 1, j).R == 255)
                {
                    ress.Add(new Point(i + 1, j));
                    prev = 3;
                }

                // Contur allocation
                n = 1;
                while (((ress[0].X != ress[n].X) || (ress[0].Y != ress[n].Y)))
                {
                    i = ress[n].X;
                    j = ress[n].Y;
                    n++;
                    switch (prev)
                    {
                        case 1:
                            if (img.GetPixel(i - 1, j - 1).R == 255)
                            {
                                ress.Add(new Point(i - 1, j - 1));
                                prev = 6;
                            }
                            else if (img.GetPixel(i - 1, j).R == 255)
                            {
                                ress.Add(new Point(i - 1, j));
                                prev = 7;
                            }
                            else if (img.GetPixel(i - 1, j + 1).R == 255)
                            {
                                ress.Add(new Point(i - 1, j + 1));
                                prev = 8;
                            }
                            else if (img.GetPixel(i, j + 1).R == 255)
                            {
                                ress.Add(new Point(i, j + 1));
                                prev = 1;
                            }
                            else if (img.GetPixel(i + 1, j + 1).R == 255)
                            {
                                ress.Add(new Point(i + 1, j + 1));
                                prev = 2;
                            }
                            else if (img.GetPixel(i + 1, j).R == 255)
                            {
                                ress.Add(new Point(i + 1, j));
                                prev = 3;
                            }
                            else if (img.GetPixel(i + 1, j - 1).R == 255)
                            {
                                ress.Add(new Point(i + 1, j - 1));
                                prev = 4;
                            }
                            break;
                        case 2:
                            if (img.GetPixel(i - 1, j).R == 255)
                            {
                                ress.Add(new Point(i - 1, j));
                                prev = 7;
                            }
                            else if (img.GetPixel(i - 1, j + 1).R == 255)
                            {
                                ress.Add(new Point(i - 1, j + 1));
                                prev = 8;
                            }
                            else if (img.GetPixel(i, j + 1).R == 255)
                            {
                                ress.Add(new Point(i, j + 1));
                                prev = 1;
                            }
                            else if (img.GetPixel(i + 1, j + 1).R == 255)
                            {
                                ress.Add(new Point(i + 1, j + 1));
                                prev = 2;
                            }
                            else if (img.GetPixel(i + 1, j).R == 255)
                            {
                                ress.Add(new Point(i + 1, j));
                                prev = 3;
                            }
                            else if (img.GetPixel(i + 1, j - 1).R == 255)
                            {
                                ress.Add(new Point(i + 1, j - 1));
                                prev = 4;
                            }
                            else if (img.GetPixel(i, j - 1).R == 255)
                            {
                                ress.Add(new Point(i, j - 1));
                                prev = 5;
                            }
                            break;
                        case 3:
                            if (img.GetPixel(i - 1, j + 1).R == 255)
                            {
                                ress.Add(new Point(i - 1, j + 1));
                                prev = 8;
                            }
                            else if (img.GetPixel(i, j + 1).R == 255)
                            {
                                ress.Add(new Point(i - 1, j + 1));
                                prev = 1;
                            }
                            else if (img.GetPixel(i + 1, j + 1).R == 255)
                            {
                                ress.Add(new Point(i + 1, j + 1));
                                prev = 2;
                            }
                            else if (img.GetPixel(i + 1, j).R == 255)
                            {
                                ress.Add(new Point(i + 1, j));
                                prev = 3;
                            }
                            else if (img.GetPixel(i + 1, j - 1).R == 255)
                            {
                                ress.Add(new Point(i + 1, j - 1));
                                prev = 4;
                            }
                            else if (img.GetPixel(i, j - 1).R == 255)
                            {
                                ress.Add(new Point(i, j - 1));
                                prev = 5;
                            }
                            else if (img.GetPixel(i - 1, j - 1).R == 255)
                            {
                                ress.Add(new Point(i - 1, j - 1));
                                prev = 6;
                            }
                            break;
                        case 4:
                            if (img.GetPixel(i, j + 1).R == 255)
                            {
                                ress.Add(new Point(i, j + 1));
                                prev = 1;
                            }
                            else if (img.GetPixel(i + 1, j + 1).R == 255)
                            {
                                ress.Add(new Point(i + 1, j + 1));
                                prev = 2;
                            }
                            else if (img.GetPixel(i + 1, j).R == 255)
                            {
                                ress.Add(new Point(i + 1, j));
                                prev = 3;
                            }
                            else if (img.GetPixel(i + 1, j - 1).R == 255)
                            {
                                ress.Add(new Point(i + 1, j - 1));
                                prev = 4;
                            }
                            else if (img.GetPixel(i, j - 1).R == 255)
                            {
                                ress.Add(new Point(i, j - 1));
                                prev = 5;
                            }
                            else if (img.GetPixel(i - 1, j - 1).R == 255)
                            {
                                ress.Add(new Point(i - 1, j - 1));
                                prev = 6;
                            }
                            else if (img.GetPixel(i - 1, j).R == 255)
                            {
                                ress.Add(new Point(i - 1, j));
                                prev = 7;
                            }
                            break;
                        case 5:
                            if (img.GetPixel(i + 1, j + 1).R == 255)
                            {
                                ress.Add(new Point(i + 1, j + 1));
                                prev = 2;
                            }
                            else if (img.GetPixel(i + 1, j).R == 255)
                            {
                                ress.Add(new Point(i + 1, j));
                                prev = 3;
                            }
                            else if (img.GetPixel(i + 1, j - 1).R == 255)
                            {
                                ress.Add(new Point(i + 1, j - 1));
                                prev = 4;
                            }
                            else if (img.GetPixel(i, j - 1).R == 255)
                            {
                                ress.Add(new Point(i, j - 1));
                                prev = 5;
                            }
                            else if (img.GetPixel(i - 1, j - 1).R == 255)
                            {
                                ress.Add(new Point(i - 1, j - 1));
                                prev = 6;
                            }
                            else if (img.GetPixel(i - 1, j).R == 255)
                            {
                                ress.Add(new Point(i - 1, j));
                                prev = 7;
                            }
                            else if (img.GetPixel(i - 1, j + 1).R == 255)
                            {
                                ress.Add(new Point(i - 1, j + 1));
                                prev = 8;
                            }
                            break;
                        case 6:
                            if (img.GetPixel(i + 1, j).R == 255)
                            {
                                ress.Add(new Point(i + 1, j));
                                prev = 3;
                            }
                            else if (img.GetPixel(i + 1, j - 1).R == 255)
                            {
                                ress.Add(new Point(i + 1, j - 1));
                                prev = 4;
                            }
                            else if (img.GetPixel(i, j - 1).R == 255)
                            {
                                ress.Add(new Point(i, j - 1));
                                prev = 5;
                            }
                            else if (img.GetPixel(i - 1, j - 1).R == 255)
                            {
                                ress.Add(new Point(i - 1, j - 1));
                                prev = 6;
                            }
                            else if (img.GetPixel(i - 1, j).R == 255)
                            {
                                ress.Add(new Point(i - 1, j));
                                prev = 7;
                            }
                            else if (img.GetPixel(i - 1, j + 1).R == 255)
                            {
                                ress.Add(new Point(i - 1, j + 1));
                                prev = 8;
                            }
                            else if (img.GetPixel(i, j + 1).R == 255)
                            {
                                ress.Add(new Point(i, j + 1));
                                prev = 1;
                            }
                            break;
                        case 7:
                            if (img.GetPixel(i + 1, j - 1).R == 255)
                            {
                                ress.Add(new Point(i + 1, j - 1));
                                prev = 4;
                            }
                            else if (img.GetPixel(i, j - 1).R == 255)
                            {
                                ress.Add(new Point(i, j - 1));
                                prev = 5;
                            }
                            else if (img.GetPixel(i - 1, j - 1).R == 255)
                            {
                                ress.Add(new Point(i - 1, j - 1));
                                prev = 6;
                            }
                            else if (img.GetPixel(i - 1, j).R == 255)
                            {
                                ress.Add(new Point(i - 1, j));
                                prev = 7;
                            }
                            else if (img.GetPixel(i - 1, j + 1).R == 255)
                            {
                                ress.Add(new Point(i - 1, j + 1));
                                prev = 8;
                            }
                            else if (img.GetPixel(i, j + 1).R == 255)
                            {
                                ress.Add(new Point(i, j + 1));
                                prev = 1;
                            }
                            else if (img.GetPixel(i + 1, j + 1).R == 255)
                            {
                                ress.Add(new Point(i + 1, j + 1));
                                prev = 2;
                            }
                            break;
                        case 8:
                            if (img.GetPixel(i, j - 1).R == 255)
                            {
                                ress.Add(new Point(i, j - 1));
                                prev = 5;
                            }
                            else if (img.GetPixel(i - 1, j - 1).R == 255)
                            {
                                ress.Add(new Point(i - 1, j - 1));
                                prev = 6;
                            }
                            else if (img.GetPixel(i - 1, j).R == 255)
                            {
                                ress.Add(new Point(i - 1, j));
                                prev = 7;
                            }
                            else if (img.GetPixel(i - 1, j + 1).R == 255)
                            {
                                ress.Add(new Point(i - 1, j + 1));
                                prev = 8;
                            }
                            else if (img.GetPixel(i, j + 1).R == 255)
                            {
                                ress.Add(new Point(i, j + 1));
                                prev = 1;
                            }
                            else if (img.GetPixel(i + 1, j + 1).R == 255)
                            {
                                ress.Add(new Point(i + 1, j + 1));
                                prev = 2;
                            }
                            else if (img.GetPixel(i + 1, j).R == 255)
                            {
                                ress.Add(new Point(i + 1, j));
                                prev = 3;
                            }
                            break;
                    }
                }
                n++;
                ress.Add(new Point(ress[1].X, ress[1].Y));
            //}
            //catch
            //{
            //    objectImageForm obj = new objectImageForm();
            //    obj.outImage = img;
            //    obj.Text = "Error";
            //    obj.Show();
            //}
            return ress;
        }
    }
}
