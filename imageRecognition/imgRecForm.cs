using System;
using System.Windows.Forms;
using System.Drawing;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace imageRecognition
{
    public partial class imgRecForm : Form
    {

        private bool DeviceExist = false;
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource = null;
        
        private Thread t1;

        System.Drawing.Image fromFile;
        string direct = Directory.GetCurrentDirectory();
        
        private Bitmap mainImage;
        Bitmap grayImage;
        List<Bitmap> objectBitmaps = new List<Bitmap>();
        double[][] vector360Contur;
        double[][] vector360Hull;
        List<System.Drawing.Point> offsetObject;
        List<Bitmap> centrImg = new List<Bitmap>();
        
        List<System.Drawing.Point> localPt = new List<System.Drawing.Point>();

        public List<double[]> contourMarks = new List<double[]>();
        public List<double[]> convexMarks = new List<double[]>();
        bool[,] bmpArr;
        List<bool[,]> listBoolImg = new List<bool[,]>();
        private List<List<System.Drawing.Point>> convexHull = new List<List<System.Drawing.Point>>();
        private List<List<System.Drawing.Point>> edgePoint = new List<List<System.Drawing.Point>>();
        //-----------------------
        //Bitmap inpBmp;
        //private Capture capt = new Capture();
        public imgRecForm()
        {
            InitializeComponent();
        }

        private void getCamList()
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                camListBox.Items.Clear();
                if (videoDevices.Count == 0) // отсутствуют устройства веб камеры
                    throw new ApplicationException();

                DeviceExist = true;
                foreach (FilterInfo device in videoDevices) //цикл по найденым видеоустройствам 
                {
                    camListBox.Items.Add(device.Name); //добавление устройств в comboBox 
                }
                camListBox.SelectedIndex = 0; //выбор устройства
            }
            catch (ApplicationException) // не найдена устройства веб камеры
            {
                DeviceExist = false;
                camListBox.Items.Add("Не найдено ни одно устройство веб камеры");
            }
        }

        private void imgRecForm_Load(object sender, EventArgs e)
        {
            getCamList();
            if (rbContur.Checked)
            {
                hullOrContur.Value = false;
            }
            else
            {
                hullOrContur.Value = true;
            }
            //if (DeviceExist) //устройство веб камеры было выбрано
            //{
            //    videoSource = new VideoCaptureDevice(videoDevices[camListBox.SelectedIndex].MonikerString);
            //    videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame); //отправка нового кадра
            //    CloseVideoSource();
            //    videoSource.Start();
            //}
            checkEtalons();
        }

        void checkEtalons()
        {
            string[] etalonImgs = Directory.GetFiles(direct + "\\etalonImg\\", "*.png");
            int k = 0;
            etalonsDgv.Rows.Clear();
            foreach (string etalon in etalonImgs)
            {
                etalonsDgv.Rows.Add();
                string[] fileNameLong = etalon.Split('\\');
                string fileNameWithExtension = fileNameLong[fileNameLong.Length - 1];
                string[] fileName = fileNameWithExtension.Split('.');
                string nameEtalon = fileName[0];
                etalonsDgv.Rows[k].Cells[0].Value = nameEtalon;
               
                fromFile = new Bitmap(Bitmap.FromFile(etalon), new Size(200, 150));

                etalonsDgv.Rows[k].Cells[1].Value = fromFile;
                k++;
            }
            if (etalonsDgv.Rows.Count == 0)
            {
                rasp.Enabled = false;
            }
            else
            {
                NamesObj.names.Clear();
                for (int i = 0; i < etalonsDgv.Rows.Count; i++)
                {
                    NamesObj.names.Add(Convert.ToString(etalonsDgv.Rows[i].Cells[0].Value));
                }
            }
        }

        private void camListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CloseVideoSource();
            if (DeviceExist) //устройство веб камеры было выбрано
            {
                videoSource = new VideoCaptureDevice(videoDevices[camListBox.SelectedIndex].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame); //отправка нового кадра
                CloseVideoSource();
                videoSource.Start();
            }
        }
        private void CloseVideoSource()
        {
            if (!(videoSource == null))
                if (videoSource.IsRunning) //проверка на запуск потока кадров веб-камеры
                {
                    videoSource.SignalToStop();
                    videoSource = null;
                }
        }
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                inputImageBox.Image = (Bitmap)eventArgs.Frame.Clone(); //захват кадра с вебкамеры
            }
            catch (VideoException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void imgRecForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseVideoSource();
        }

        private Bitmap binarize(Bitmap imgToBinarize)
        {
            try
            {
                OtsuThreshold binarizeFilter = new OtsuThreshold();
                binarizeFilter.ApplyInPlace(imgToBinarize);
                Invert invertFilter = new Invert();
                invertFilter.ApplyInPlace(imgToBinarize);
            }
            catch (StackOverflowException)
            {
                MessageBox.Show("Невозиожно бинаризовать");
            }
            catch { MessageBox.Show("Непредвиденная ошибка"); }
            return imgToBinarize;
        }

        private Rectangle[] recurBlobCounter(Bitmap binariImg)
        {
            Rectangle[] rects = new Rectangle[0] ;
            try
            {
                BlobCounter bc = new BlobCounter();
                bc.ProcessImage(binariImg);
                rects = bc.GetObjectsRectangles();
            }
            catch(StackOverflowException)
            {
                MessageBox.Show("Невозиожно бинаризовать");
            }
            catch { MessageBox.Show("Непредвиденная ошибка"); }
            return rects;
        }

        private void getSingleObj(Rectangle[] rects, Bitmap binariImg)
        {
            clearNull();
            //----------------------------
            objectImageForm OBJ = new objectImageForm();
            procImg pi = new procImg();
            centrImg = new List<Bitmap>();
            listBoolImg = new List<bool[,]>();
            offsetObject = new List<System.Drawing.Point>();
            int offsetX = 0, offsetY = 0; int klass = 0;
            Bitmap bp;
            //-----------------------------
            int i = 0;
            foreach (Rectangle rect in rects)
            {
                bmpArr = new bool[mainImage.Width, mainImage.Height];
                for (int x = 0; x < rect.Width; x++) {
                    for (int y = 0; y < rect.Height; y++)  {
                        if (mainImage.GetPixel(x + rect.Location.X, y + rect.Location.Y).R == 255)
                        {    bmpArr[x + rect.Location.X, y + rect.Location.Y] = true;     }
                        else   {
                            bmpArr[x + rect.Location.X, y + rect.Location.Y] = false; }  }  }
                listBoolImg.Add(bmpArr);
                offsetX = ((int)(mainImage.Width / 2)) - (int)(rect.Location.X + ((rect.Width) / 2));
                offsetY = ((int)(mainImage.Height / 2)) - (int)(rect.Location.Y + ((rect.Height) / 2));
                offsetObject.Add(new System.Drawing.Point((int)offsetX, (int)offsetY));
                objectBitmaps.Add(new Bitmap(binariImg.Width, binariImg.Height));
                Graphics g = Graphics.FromImage(objectBitmaps[i]);   //получаю объект графики из битмап
                SolidBrush b = new SolidBrush(Color.Black);  //кисть для заливки
                g.FillRectangle(b, new Rectangle(0, 0, objectBitmaps[i].Width, objectBitmaps[i].Height)); //заполняю
                bp = binariImg.Clone(rects[i], System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                bp.Save("D:\\6.png");
                g.DrawImage(bp, rects[i].X, rects[i].Y, rects[i].Width, rects[i].Height);
                i++;  g = null; b = null;
            }
            rects = null;


            foreach (Bitmap img in objectBitmaps)
            {
                centrImg.Add(new Bitmap(img.Width, img.Height));
            }

            for (int k = 0; k < centrImg.Count; k++) { 
                for (int x = 0; x < centrImg[k].Width; x++)  {
                    for (int y = 0; y < centrImg[k].Height; y++)  {
                        if (listBoolImg[k][x, y] == true)  {
                            centrImg[k].SetPixel(x + offsetObject[k].X, y + offsetObject[k].Y, Color.White);
                        }
                    }
                }
            }
            foreach (Bitmap bmp in centrImg)
            {
                Bitmap bmp1 = bmp.Clone(new Rectangle(0, 0, grayImage.Width, grayImage.Height),
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                counterAndConvex(bmp1, klass);
                Invert invert = new Invert();
                invert.ApplyInPlace(bmp1);
                klass++;
            }
            offsetObject = new List<Point>();
        }

        private void clearNull()
        {
            objectBitmaps.Clear();
            vector360Contur = null;
            vector360Hull = null;
            centrImg.Clear();
            contourMarks.Clear();
            convexMarks.Clear();
            bmpArr = null;
            listBoolImg.Clear();
            convexHull.Clear();
            edgePoint.Clear();
            localPt.Clear();      
    }


        private void counterAndConvex(Bitmap Image, int klass)
        {
            //---------------------------------------------------
            int angle = 180;
            Discriminate discrAnalize = new Discriminate();
            double[] contur = new double[18], hull = new double[28] ;
            procImg pi = new procImg();
            QuickHull qh = new QuickHull();
            objectImageForm OBJ = new objectImageForm();
            Contur cont = new Contur();
            localPt = new List<System.Drawing.Point>();
            //----------------------------------------------------
            vector360Contur = MatrixCreate(360, 18);
            vector360Hull = MatrixCreate(360, 28);
            
            if (learnOrRecognize.Value)
            {
                Bitmap ImageForThread = Image.Clone(
                          new Rectangle(0, 0, Image.Width, Image.Height),
                          System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                
                t1 = new Thread(() => VectorsRotate180(ImageForThread, 180, 360, t1));
                t1.Start();

                for (int i = 0; i < angle; i++)
                {
                    //поворот на градус i
                    Bitmap tempImage = pi.RotateImg(Image, i);
                    try
                    {
                        Median filter = new Median();
                        filter.ApplyInPlace(tempImage);
                        tempImage = pi.ProcImg(tempImage);
                        localPt = pi.GetPoints(tempImage);
                        List<System.Drawing.Point> ConvexHullLocal = qh.quickHull(localPt);
                        ConvexHullLocal = qh.DeleteAnglePoints(ConvexHullLocal);
                        List<System.Drawing.Point> ConturLocal = cont.kontur(tempImage);
                        ConturLocal = cont.DeleteAnglePoints(ConturLocal);
                        //выделение выпуклой оболочки
                        Primary marks = new Primary(tempImage, ConturLocal, ConvexHullLocal);
                        contur = marks.Contour(); hull = marks.Convex();
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка при построении контура");
                        break;
                    }

                    for (int j = 0; j < hull.Length; j++)
                        vector360Hull[i][j] = hull[j];
                    for (int j = 0; j < contur.Length; j++)
                        vector360Contur[i][j] = contur[j];
                    progressBar1.Value = i + 1;
                }
                OBJ = new objectImageForm();
                OBJ.contour = vector360Contur;
                OBJ.convex = vector360Hull;
                Bitmap outImg = changeImg(Image, klass);
                OBJ.outImage = outImg;
                OBJ.ShowDialog();
                
            }            
            else
            {
                Bitmap tempImage = Image;
                try
                {
                    localPt = pi.GetPoints(tempImage);
                    List<System.Drawing.Point> ConvexHullLocal = qh.quickHull(localPt);
                    ConvexHullLocal = qh.DeleteAnglePoints(ConvexHullLocal);
                    List<System.Drawing.Point> ConturLocal = cont.kontur(tempImage);
                    ConturLocal = cont.DeleteAnglePoints(ConturLocal);
                    //выделение выпуклой оболочки
                    Primary marks = new Primary(tempImage, ConturLocal, ConvexHullLocal);
                    contur = marks.Contour(); hull = marks.Convex();
                }
                catch
                {
                    MessageBox.Show("Ошибка при построении контура");
                    
                }
                Image = pi.RotateImg(Image, 120);
                Image.Save("D:\\7.png");
                Bitmap outImg = changeImg(Image, klass);
                
                if (!hullOrContur.Value)
                    if (rbFisher.Checked)
                        discrAnalize.calcDiscrFisher(contur, outImg, 18);
                    else
                        discrAnalize.calcLinearDiscr(contur, outImg, 18);
                else
                    if (rbFisher.Checked)
                        discrAnalize.calcDiscrFisher(hull, outImg, 28);
                    else
                        discrAnalize.calcLinearDiscr(hull, outImg, 28);
            }
            
        }

        Bitmap changeImg(Bitmap Image, int klass)
        {
            Bitmap outImg = new Bitmap(Image.Width, Image.Height);
            for (int x = 0; x < outImg.Width; x++)
                for (int y = 0; y < outImg.Height; y++)
                    if (listBoolImg[klass][x, y] == true)
                    {
                        Bitmap b = (Bitmap)inputImageBox.Image.Clone();
                        Color c = Color.FromArgb(b.GetPixel(x, y).R, b.GetPixel(x, y).G, b.GetPixel(x, y).B);
                        outImg.SetPixel(x + offsetObject[klass].X, y + offsetObject[klass].Y, c);
                    }
            outImg = new Bitmap(outImg, new Size(341, 256));
            return outImg;
        }

        public void VectorsRotate180(Bitmap image, int begin, int end, Thread t1)
        {
            QuickHull qh = new QuickHull();
            Contur cont1 = new Contur();
            int i;
            double[] contur = new double[18], hull = new double[28];
            procImg pi = new procImg();
            List<System.Drawing.Point> localPt;
            for (i = begin; i < end; i++)
            {
                Bitmap tempImg = pi.RotateImg(image, i);
                try
                {
                    Median filter = new Median();
                    filter.ApplyInPlace(tempImg);
                    tempImg = pi.ProcImg(tempImg);
                    localPt = pi.GetPoints(tempImg);
                    //выделение выпуклой оболочки
                    List<System.Drawing.Point> ConvexHullLocal = qh.quickHull(localPt);
                    ConvexHullLocal = qh.DeleteAnglePoints(ConvexHullLocal);
                    //и контура
                    List<System.Drawing.Point> ConturLocal = cont1.kontur(tempImg);
                    ConturLocal = cont1.DeleteAnglePoints(ConturLocal);
                    Primary marks = new Primary(tempImg, ConturLocal, ConvexHullLocal);
                    contur = marks.Contour(); hull = marks.Convex();
                }
                catch
                {                    
                    MessageBox.Show("Ошибка при построении контура");
                    break;
                }
                for (int j = 0; j < hull.Length; j++)
                {
                    vector360Hull[i][j] = hull[j];
                }
                for (int j = 0; j < contur.Length; j++)
                {
                    vector360Contur[i][j] = contur[j];
                }
            }
            t1.Abort();
        }


        private void rasp_Click(object sender, EventArgs e)
        {
            rasp.Enabled = false;
            learnBtn.Enabled = false;
            learnOrRecognize.Value = false;
            camListBox.Enabled = false;        
            Recognition();
            resetBtn.Enabled = true;
            camListBox.Enabled = true;
            rasp.Enabled = true;
        }


        private void learnBtn_Click(object sender, EventArgs e)
        {
            rasp.Enabled = false;
            camListBox.Enabled = false;
            learnBtn.Enabled = false;
            learnOrRecognize.Value = true;
            Recognition();
            checkEtalons();
            resetBtn.Enabled = true;
            camListBox.Enabled = true;
            rasp.Enabled = true;

        }

        public void Recognition()
        {
            //-----------------------------------
            CloseVideoSource();            
            objectImageForm OBJ = new objectImageForm();
            //-----------------------------------

            grayImage = (Bitmap)inputImageBox.Image;
            grayImage.Save("D:\\1.png");
            // create filter
            Median fMedian = new Median();
            // apply the filter
            fMedian.ApplyInPlace(grayImage);
            grayImage.Save("D:\\2.png");
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            grayImage = filter.Apply(grayImage);
            grayImage.Save("D:\\3.png");
            fMedian.ApplyInPlace(grayImage);
            mainImage = grayImage.Clone(
                    new Rectangle(0, 0, grayImage.Width, grayImage.Height),
                    System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            bmpArr = new bool[mainImage.Width, mainImage.Height];
            mainImage = binarize(mainImage);
            mainImage.Save("D:\\4.png");
            Rectangle[] rects = recurBlobCounter(mainImage);
            getSingleObj(rects, mainImage); //получаем отдельные изображения
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            resetBtn.Enabled = false;
            rasp.Enabled = true;
            learnBtn.Enabled = true;

            getCamList();
            if (DeviceExist) //устройство веб камеры было выбрано
            {
                videoSource = new VideoCaptureDevice(videoDevices[camListBox.SelectedIndex].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame); //отправка нового кадра
                CloseVideoSource();
                videoSource.Start();
            }
        }
               
        static double[][] MatrixCreate(int rows, int cols)
        {
            // Создаем матрицу, полностью инициализированную
            // значениями 0.0.
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[cols]; // автоинициализация в 0.0
            return result;
        }

        private void rbHull_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHull.Checked)
            {
                hullOrContur.Value = true;
            }
            else
            {
                hullOrContur.Value = false;
            }
        }

        private void rbContur_CheckedChanged(object sender, EventArgs e)
        {
            if (rbContur.Checked)
            {
                hullOrContur.Value = false;
            }
            else
            {
                hullOrContur.Value = true;
            }
        }

        private void camListBox_Click(object sender, EventArgs e)
        {

        }
    }
}
