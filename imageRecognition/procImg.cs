using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace imageRecognition
{
    class procImg
    {
        public List<Point> GetPoints(Bitmap bmp)
        {
            List<Point> Points = new List<Point>();

            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    if (bmp.GetPixel(x, y).R >= 155)
                        Points.Add(new Point(x, y));
                }

            return Points;
        }
        public Bitmap RotateImg(Bitmap bp, int angle)
        {
            RotateBilinear filter = new RotateBilinear(angle, true);
            // apply the filter
            Bitmap newImage = filter.Apply(bp);
            return newImage;
        }
        public Bitmap ProcImg(Bitmap source)
        {
            for (int x = 0; x < source.Width; x++)
                for (int y = 0; y < source.Height; y++)
                {
                    if (source.GetPixel(x, y).R != 0)
                    {
                        source.SetPixel(x, y, Color.White);
                    }
                }
            return source;
        }
    }
}
