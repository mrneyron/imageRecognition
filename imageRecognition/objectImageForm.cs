using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace imageRecognition
{
    public partial class objectImageForm : Form
    {
        public Bitmap outImage;

        public double[][] contour = new double[360][];
        public double[][] convex = new double[360][]; public int klas;

        public double Hk; public double delitel;
        string direct = Directory.GetCurrentDirectory();
        public objectImageForm()
        {
            InitializeComponent();
        }

        private void objectImageForm_Load(object sender, EventArgs e)
        {
            picBox.Width = (int)(outImage.Width);
            picBox.Height = (int)(outImage.Height);
            this.Width = (int)(outImage.Width) + 234;
            this.Height = (int)(outImage.Height) + 64;
            outInformation.Location = new Point((int)(outImage.Width) + 18, 28);
            outInformation.Height = this.Height - 80;
            label1.Location = new Point((int)(outImage.Width) + 19, 12);

            if (learnOrRecognize.Value)
            {
                outInformation.Height = this.Height - 147;
                label2.Location = new Point((int)(outImage.Width) + 19, outInformation.Height + 31);
                nameBox.Location = new Point((int)(outImage.Width) + 18, outInformation.Height + 47);
                saveBtn.Location = new Point((int)(outImage.Width) + 18, outInformation.Height + 73);
                progressBar1.Location = new Point(0, picBox.Height + 19);
                progressBar1.Height = outImage.Width+218;
                this.Height = (int)(outImage.Height) + 83;
                checkName();   
            }
            else
            {
                progressBar1.Visible = false;
                label2.Visible = false;
                nameBox.Visible = false;
                saveBtn.Visible = false;
                label2.Enabled = false;
                nameBox.Enabled = false;
                saveBtn.Enabled = false;
                outInformation.Text += "Объект относится к классу: " + NamesObj.names[klas];
                //outInformation.Text += '\n' + "Функция равна: " + '\n' + "1/"+ delitel + "* e^" + Hk;
            }
            picBox.Image = outImage;
            outImage.Save("D:\\w.png");
        }
        static string MatrixAsString(double[][] matrix)
        {
            string s = "";
            for (int i = 0; i < matrix.Length; ++i)
            {
                for (int j = 0; j < matrix[i].Length; ++j)
                    s += matrix[i][j].ToString("F9") + " ";
                s += Environment.NewLine;
            }
            return s;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < contour.Length; i++)
            {
                using (StreamWriter sw = new StreamWriter(direct + "\\vectorsContur\\" + nameBox.Text +
                           "_Contur" + ".txt", true, System.Text.Encoding.Default))
                {
                    for (int j = 0; j < contour[i].Length; j++)
                    {
                        sw.Write(contour[i][j] + ";");
                        progressBar1.Value = i;
                    }
                    sw.Write(":" + '\n');
                }
            }
            for (int i = 0; i < convex.Length; i++)
            {
                using (StreamWriter sw = new StreamWriter(direct + "\\vectorsHull\\" + nameBox.Text +
                       "_Hull" + ".txt", true, System.Text.Encoding.Default))
                {
                    for (int j = 0; j < convex[i].Length; j++)
                    {
                        sw.Write(convex[i][j] + ";");
                        progressBar1.Value = 360 + i;
                    }
                    sw.Write(":"+'\n');
                }
            }
            outImage.Save(direct + "\\etalonImg\\" + nameBox.Text + ".png");
            
            Discriminate discrAnalize = new Discriminate();
            double[] middleContur = discrAnalize.arrayMiddleVector(contour);
            double[] middleHull = discrAnalize.arrayMiddleVector(convex);
            for (int i = 0; i < middleHull.Length; i++)
            {
                using (StreamWriter sw = new StreamWriter(direct + "\\Middle\\" + nameBox.Text +
                      "_Hull" + ".txt", true, System.Text.Encoding.Default))
                {
                    sw.Write(middleHull[i] + ";");
                }
            }

            for (int i = 0; i < middleContur.Length; i++)
            {
                using (StreamWriter sw = new StreamWriter(direct + "\\Middle\\" + nameBox.Text +
                      "_Contur" + ".txt", true, System.Text.Encoding.Default))
                {
                    sw.Write(middleContur[i] + ";");
                }
            }

            double[][] matrixChull = calcC(convex, middleHull, 28);
            double[][] matrixCcontur = calcC(contour, middleContur, 18);
            for (int i = 0; i < matrixChull.Length; i++)
            {
                for (int j = 0; j < matrixChull[i].Length; j++)
                {
                    using (StreamWriter sw = new StreamWriter(direct + "\\matrixC\\" + nameBox.Text +
                      "_Hull" + ".txt", true, System.Text.Encoding.Default))
                    {
                        sw.Write(matrixChull[i][j] + ";");
                    }
                }
            }
            for (int i = 0; i < matrixCcontur.Length; i++)
            {
                for (int j = 0; j < matrixCcontur[i].Length; j++)
                {
                    using (StreamWriter sw = new StreamWriter(direct + "\\matrixC\\" + nameBox.Text +
                      "_Contur" + ".txt", true, System.Text.Encoding.Default))
                    {
                        sw.Write(matrixCcontur[i][j] + ";");
                    }
                }
            }
            discrAnalize.writeHk(18, nameBox.Text);
            discrAnalize.writeHk(28, nameBox.Text);
            MessageBox.Show("Эталон успешно сохранен");
            Close();
        }

        static double[][] calcC(double[][] Vectors, double[] mid, int size)
        {
            double[][] matrixC = MatrixCreate(size, size);
            double xim = 0, Xi = 0, xjm = 0, Xj;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int m = 0; m < 360; m++)
                    {
                        xim = Vectors[m][i];
                        Xi = mid[i];
                        xjm = Vectors[m][j];
                        Xj = mid[j];
                        matrixC[i][j] += (xim - Xi) * (xjm - Xj);
                    }
                    matrixC[i][j] = matrixC[i][j] / (360 - 1);
                }
            }
            return matrixC;
        }

        static double[][] MatrixCreate(int rows, int cols)
        {
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[cols];
            return result;
        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            checkName();
        }
        private void checkName()
        {
            if (nameBox.Text == "")
            {
                saveBtn.Enabled = false;
                saveBtn.Text = "Введите наименование!";
            }
            else
            {
                saveBtn.Enabled = true;
                saveBtn.Text = "Сохранить эталон";
            }
        }
    }
}
