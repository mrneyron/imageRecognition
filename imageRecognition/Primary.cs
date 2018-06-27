using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace imageRecognition
{
    public class Primary
    {
        private double M1, M2, M3, M4, K, T, Lkont = 0, S0, P0, Li, Lj, Lvyp, Lvog, Llin;
        private double Sconv, Sdiff, Pdiff, Lkontconv,P0conv, M1c, M2c, M3c, M4c, Kc, Tc, LvyP0conv, Lvogvyp, Llinconv;
        
        private List<Point> _contur;
        private List<Point> _convexHull;

        private Bitmap _img;

        public Bitmap filledContour;
        public Bitmap filledConvex;


        

        public Primary(Bitmap img, List<System.Drawing.Point> contur, List<System.Drawing.Point> convexHull)
        {
            this._img = img;
            this._contur = contur;
            this._convexHull = convexHull;

            filledContour = new Bitmap(_img);
            filledConvex = new Bitmap(_img);

            getPrimary();            
        }

        
   
        public void getPrimary()
        {
            S0 = getS0cont();
            //общее количество точек, образуюших контур
            Lkont = getLkont(_contur);
            P0 = getM1234KT(_contur, "contour");
            double b =Math.Sqrt(2);
            Li = 2 * b; //2*b
            Lj = 1 + b; //a+b
            Lvyp = 0.5 * (M1 * Li + M3 + Lj);
            Lvog = 0.5 * (M2 * Li + M4 * Lj);
            Llin = 0.5 * (K * 2 + T * 2 * b);

            //признаки cуклой оболочки
            Sconv = getS0conv(); 
            Sdiff = Sconv - S0;
            Lkontconv = getLkont(_convexHull);
            P0conv = getM1234KT(_convexHull, "convex");
            Pdiff = P0 - P0conv;
            LvyP0conv = 0.5 * (M1c * Li + M3c + Lj);
            Lvogvyp = 0.5 * (M2c * Li + M4c * Lj);
            Llinconv = 0.5 * (Kc * 2 + Tc * 2 * b);
        }

        private double getS0cont()
        {
            double ploshad = 0;
            using (Graphics g1 = Graphics.FromImage(filledContour))
            {
                //рисуем                
                g1.FillPolygon(new SolidBrush(Color.White), _contur.ToArray(), FillMode.Winding);
            }

            for (int i = 0; i < filledContour.Height; i++)
                for (int j = 0; j < filledContour.Width; j++)
                {
                    if (255 * filledContour.GetPixel(j, i).GetBrightness() == 255)
                        ploshad++; //площадь обьекта
                }

            return ploshad;
        }

        private double getS0conv()
        {
            double ploshad = 0;

            using (Graphics g1 = Graphics.FromImage(filledConvex))
            {
                //рисуем                
                g1.FillPolygon(new SolidBrush(Color.White), _convexHull.ToArray(), FillMode.Winding);
            }

            for (int i = 0; i < filledConvex.Height; i++)
                for (int j = 0; j < filledConvex.Width; j++)
                {
                    if (255 * filledConvex.GetPixel(j, i).GetBrightness() == 255)
                        ploshad++; //площадь обьекта
                }

            return ploshad;
        }

        private double getM1234KT(List<Point> points, string flag)
        {
            //mask's for +90 degree
            int[,] plus901 = new int[,] { { 1, 1, 1 }, { 0, 1, 0 }, { 0, 0, 0 } };
            int[,] plus902 = new int[,] { { 0, 0, 1 }, { 0, 1, 1 }, { 0, 0, 1 } };
            int[,] plus903 = new int[,] { { 0, 0, 0 }, { 0, 1, 0 }, { 1, 1, 1 } };
            int[,] plus904 = new int[,] { { 1, 0, 0 }, { 1, 1, 0 }, { 1, 0, 0 } };
            // mask's for -90 degree
            int[,] minus901 = new int[,] { { 1, 0, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            int[,] minus902 = new int[,] { { 1, 1, 1 }, { 1, 1, 0 }, { 1, 1, 1 } };
            int[,] minus903 = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 0, 1 } };
            int[,] minus904 = new int[,] { { 1, 1, 1 }, { 0, 1, 1 }, { 1, 1, 1 } };
            // mask's for +135 degree
            int[,] plus1351 = new int[,] { { 0, 1, 1 }, { 0, 1, 1 }, { 0, 0, 1 } };
            int[,] plus1352 = new int[,] { { 0, 0, 1 }, { 0, 1, 1 }, { 0, 1, 1 } };
            int[,] plus1353 = new int[,] { { 0, 0, 0 }, { 0, 1, 1 }, { 1, 1, 1 } };
            int[,] plus1354 = new int[,] { { 0, 0, 0 }, { 1, 1, 0 }, { 1, 1, 1 } };
            int[,] plus1355 = new int[,] { { 1, 0, 0 }, { 1, 1, 0 }, { 1, 1, 0 } };
            int[,] plus1356 = new int[,] { { 1, 1, 0 }, { 1, 1, 0 }, { 1, 0, 0 } };
            int[,] plus1357 = new int[,] { { 1, 1, 1 }, { 1, 1, 0 }, { 0, 0, 0 } };
            int[,] plus1358 = new int[,] { { 1, 1, 1 }, { 0, 1, 1 }, { 0, 0, 0 } };
            // mask's for -135 degree
            int[,] minus1351 = new int[,] { { 1, 1, 0 }, { 1, 1, 0 }, { 1, 1, 1 } };
            int[,] minus1352 = new int[,] { { 1, 1, 1 }, { 1, 1, 0 }, { 1, 1, 0 } };
            int[,] minus1353 = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 0, 0 } };
            int[,] minus1354 = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 0, 0, 1 } };
            int[,] minus1355 = new int[,] { { 1, 1, 1 }, { 0, 1, 1 }, { 0, 1, 1 } };
            int[,] minus1356 = new int[,] { { 0, 1, 1 }, { 0, 1, 1 }, { 1, 1, 1 } };
            int[,] minus1357 = new int[,] { { 0, 0, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            int[,] minus1358 = new int[,] { { 1, 0, 0 }, { 1, 1, 1 }, { 1, 1, 1 } };
            // mask's for linear
            int[,] linK1 = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 0, 0, 0 } };
            int[,] linK2 = new int[,] { { 0, 0, 0 }, { 1, 1, 1 }, { 1, 1, 1 } };
            int[,] linK3 = new int[,] { { 1, 1, 0 }, { 1, 1, 0 }, { 1, 1, 0 } };
            int[,] linK4 = new int[,] { { 0, 1, 1 }, { 0, 1, 1 }, { 0, 1, 1 } };
            int[,] diagT1 = new int[,] { { 1, 1, 1 }, { 0, 1, 1 }, { 0, 0, 1 } };
            int[,] diagT2 = new int[,] { { 1, 0, 0 }, { 1, 1, 0 }, { 1, 1, 1 } };
            int[,] diagT3 = new int[,] { { 1, 1, 1 }, { 1, 1, 0 }, { 1, 0, 0 } };
            int[,] diagT4 = new int[,] { { 0, 0, 1 }, { 0, 1, 1 }, { 1, 1, 1 } };

            bool m1351, m1352, m1353, m1354, m1355, m1356, m1357, m1358;
            bool mlinK1, mlinK2, mlinK3, mlinK4;
            bool mdiagT1, mdiagT2, mdiagT3, mdiagT4;
            bool m901, m902, m903, m904;

            foreach (Point point in points)
            {
                int[,] imgPart = new int[,] { {  (int)_img.GetPixel(point.X - 1, point.Y + 1).GetBrightness(),  (int)_img.GetPixel(point.X, point.Y + 1).GetBrightness(),  (int)_img.GetPixel(point.X + 1, point.Y + 1).GetBrightness() }, 
                {  (int)_img.GetPixel(point.X - 1, point.Y).GetBrightness(),  (int)_img.GetPixel(point.X, point.Y).GetBrightness(),  (int)_img.GetPixel(point.X + 1, point.Y).GetBrightness() }, 
                {  (int)_img.GetPixel(point.X - 1, point.Y - 1).GetBrightness(),  (int)_img.GetPixel(point.X, point.Y - 1).GetBrightness(),  (int)_img.GetPixel(point.X + 1, point.Y - 1).GetBrightness() } };

                 m1351 = Equal(plus1351, imgPart); 
                 m1352 = Equal(plus1352, imgPart); 
                 m1353 = Equal(plus1353, imgPart); 
                 m1354 = Equal(plus1354, imgPart); 
                 m1355 = Equal(plus1355, imgPart); 
                 m1356 = Equal(plus1356, imgPart); 
                 m1357 = Equal(plus1357, imgPart); 
                 m1358 = Equal(plus1358, imgPart);

                if (m1351 || m1352 || m1353 || m1354 || m1355 || m1356 || m1357 || m1358)
                {
                    if (flag == "contour")
                        M3++;
                    else if (flag == "convex")
                        M3c++;
                }

                m1351 = Equal(minus1351, imgPart);
                m1352 = Equal(minus1352, imgPart);
                m1353 = Equal(minus1353, imgPart);
                m1354 = Equal(minus1354, imgPart);
                m1355 = Equal(minus1355, imgPart);
                m1356 = Equal(minus1356, imgPart);
                m1357 = Equal(minus1357, imgPart);
                m1358 = Equal(minus1358, imgPart);
                if (m1351 || m1352 || m1353 || m1354 || m1355 || m1356 || m1357 || m1358)
                {
                    if (flag == "contour")
                        M4++;
                    else if (flag == "convex")
                        M4c++;
                }

                mlinK1 = Equal(linK1, imgPart);
                mlinK2 = Equal(linK2, imgPart);
                mlinK3 = Equal(linK3, imgPart);
                mlinK4 = Equal(linK4, imgPart);
                if (mlinK1 || mlinK2 || mlinK3 || mlinK4)
                {
                    if (flag == "contour")
                        K++;
                    else if (flag == "convex")
                        Kc++;
                }

                mdiagT1 = Equal(diagT1, imgPart);
                mdiagT2 = Equal(diagT2, imgPart);
                mdiagT3 = Equal(diagT3, imgPart);
                mdiagT4 = Equal(diagT4, imgPart);
                if (mdiagT1 || mdiagT2 || mdiagT3 || mdiagT4)
                {
                    if (flag == "contour")
                        T++;
                    else if (flag == "convex")
                        Tc++;
                }

                m901 = Equal(plus901, imgPart);
                m902 = Equal(plus902, imgPart);
                m903 = Equal(plus903, imgPart);
                m904 = Equal(plus904, imgPart);
                if (m901 || m902 || m903 || m904)
                {
                    if (flag == "contour")
                        M1++;
                    else if (flag == "convex")
                        M1c++;
                }

                m901 = Equal(minus901, imgPart);
                m902 = Equal(minus902, imgPart);
                m903 = Equal(minus903, imgPart);
                m904 = Equal(minus904, imgPart);
                if (m901 || m902 || m903 || m904)
                {
                    if (flag == "contour")
                        M2++;
                    else if (flag == "convex")
                        M2c++;
                }
            }


            if (flag == "contour")
                return M1 + M2 + M3 + M4 + K + T;
            else if (flag == "convex")
                return M1c + M2c + M3c + M4c + Kc + Tc;
            else return 241234543;            
        }

        private bool Equal(int[,] mass1, int[,]mass2)
        {
            int sum = 0;
            for (int i = 0; i < mass1.GetLength(0); i++)
                    for (int j = 0; j < mass1.GetLength(1); j++)
                    {
                        if (mass1[i, j] == mass2[i, j])
                            sum++;
                    }
            if (sum == 9)
                return true;
            else 
                return false;
        }

        private double getLkont(List<Point> points)
        {
            int lin = 0, diag = 0;
            bool N15, N37, N2468;
            for (int k = 0; k < points.Count-1; k++)
            {
                 N15 = points[k].X == points[k+1].X && Math.Abs(points[k].Y -points[k+1].Y) == 1;
                 N37 = Math.Abs(points[k].X - points[k + 1].X) == 1 && points[k].Y == points[k + 1].Y;

                if (N15 || N37)
                 lin++;

                 N2468 = Math.Abs(points[k].X - points[k + 1].X) == 1 && Math.Abs(points[k].Y - points[k + 1].Y) == 1;

                if (N2468)
                    diag++;
            }

            //check the last point
            int end = points.Count-1;
            int start = 0;

             N15 = points[end].X == points[start].X && Math.Abs(points[end].Y - points[start].Y) == 1;
             N37 = Math.Abs(points[end].X - points[start].X) == 1 && points[end].Y == points[start].Y;

            if (N15 || N37)
                lin++;

            N2468 = Math.Abs(points[end].X - points[start].X) == 1 && Math.Abs(points[end].Y - points[start].Y) == 1;

            if (N2468)
                diag++;

            return lin + Math.Sqrt(2) * diag;
        }

        public double[] Contour()
        {
            double[] marks = new double[] { 
            P0/S0,//1
            M1 / S0,//2
            M2/S0,//3
            M3/S0,//4
            M4/S0,//5
            K / S0,//6
            T / S0,//7
            M1 / P0,//8
            M2 / P0,//9
            M1 / (M1 + M2 + M3 + M4),//17
            M2 / (M1 + M2 + M3 + M4),//18
            M3 / (M1 + M2 + M3 + M4),//19
            M4 / (M1 + M2 + M3 + M4),//20
            (M1 + M2 + M3 + M4) / (P0 + S0 + K + T),//24
            (K + T) / (P0 + S0),//25
            (M1 + M2 + M3 + M4) / (P0 + S0),//26
            ((M1 + M2 + M3 + M4) * K) / (P0 * S0),//27
            ((M1 + M2 + M3 + M4) * T) / (P0 * S0), };//28

            return marks; 
        }

        public double[] Convex()
        {
            double[] marks = new double[] { 
                S0 / Sconv,//1
                P0 / Sconv,//2
                Sdiff / Sconv,//3
                P0conv / Sconv,//4
                M1c / Sconv,//5
                M3c / Sconv,//6
                M4c / Sconv,//7
                Kc / Sconv,//8
                Tc / Sconv,//9
                Sdiff / S0,//10
                P0conv / S0,//11
                M1c / S0,//12
                M3c / S0,//13
                M4c / S0,//14
                Kc / S0,//15
                Tc / S0,//16
                P0conv / P0,//17
                M1c / P0conv,//18
                M1c / P0,//23
                Lkontconv / Lkont,//28
                M1c / (M1c + M3c + M4c),//35
                M3c / (M1c + M3c + M4c),//36
                M4c / (M1c + M3c + M4c),//37
                (Kc + Tc) / (P0conv + Sconv),//41
                (M1c + M3c + M4c) / (P0conv + Sconv),//42
                ((M1c + M3c + M4c) * Kc) / (P0conv * Sconv),//43
                ((M1c + M3c + M4c) * Tc) / (P0conv * Sconv),//44
                (M1c * M3c * M4c) / ((Tc + Kc) * P0conv),//45
            };

            return marks;
        }

        public double[] ConvexError()
        {
            double[] marks1 = new double[] {
                S0,Sconv,M4c
            };
            return marks1;
        }
    }
}
