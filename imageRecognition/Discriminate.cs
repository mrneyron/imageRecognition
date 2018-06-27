using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Accord.Math;
using System.IO;


namespace imageRecognition
{
    class Discriminate
    {
        public List<double[][]> vectrosFromFile = new List<double[][]>();
        public List<double[]> arraysBFromFile = new List<double[]>();
        public List<double[][]> matrixsCFromFile = new List<double[][]>();
        List<double[]> middleVectors = new List<double[]>();

        List<double> Hks = new List<double>();
        string direct = Directory.GetCurrentDirectory();

        

        public void calcLinearDiscr(double[] nowVector, Bitmap bmp, int size)
        {
            double Fk = 0.0d;
            List<double> Estep = new List<double>();
            List<double> delitel = new List<double>();
            List<double[][]> matrixCs = new List<double[][]>();
            double[][] matrixC;
            List <double[][]> matrixAs = new List<double[][]>();
            double[][] matrixA;
            readMatrixC(size);
            readMiddleVectors(size);

            for (int i = 0; i < matrixsCFromFile.Count; i++)
            {
                matrixC = matrixsCFromFile[i];
                matrixA = Matrix.PseudoInverse(matrixC);
                matrixCs.Add(matrixC);
                matrixAs.Add(matrixA);
                matrixA = null;
                matrixC = null;
            }
            double stepE = 0, delit = 0;
            for (int k = 0; k < middleVectors.Count; k++)
            {
                for (int i = 0; i < matrixCs[0].Length; i++)
                {
                    for (int j = 0; j < matrixCs[0].Length; j++)
                    {
                        stepE += (-1.0 / 2.0) * (nowVector[i] - middleVectors[k][i]) * matrixAs[k][i][j] * (nowVector[j] - middleVectors[k][j]);
                        delit += Math.Pow((2.0 * Math.PI), (nowVector.Length / 2.0)) * Math.Pow(Math.Abs(matrixCs[k][i][j]), 0.5);
                    }
                }
                Fk = Math.Pow(Math.E, stepE) / delit;
                //Fks.Add(stepE); 
                Estep.Add(stepE); delitel.Add(delit);
                delit = 0; stepE = 0;
            }

            objectImageForm OBJ = new objectImageForm();
            int ind = Estep.LastIndexOf(Estep.Max());
            int index = delitel.LastIndexOf(delitel.Min());
            if (!(ind == index))
                OBJ.klas = ind;
            else
                OBJ.klas = index;
            OBJ.outImage = bmp;
            OBJ.Hk = Estep[ind];
            OBJ.delitel = delitel[ind];
            OBJ.Show();
            Fk = 0.0d; matrixC = null; matrixA = null; middleVectors = new List<double[]>(); 
            matrixAs = new List<double[][]>(); Hks = null; matrixCs = new List<double[][]>();
            matrixsCFromFile = new List<double[][]>();
        }

        public void calcDiscrFisher(double[] nowVector, Bitmap bmp, int size)
        {
            double Hk = 0.0;
            readArrayB(size);
            objectImageForm OBJ = new objectImageForm();
            for (int k = 0; k < arraysBFromFile.Count; k++)
            {
                Hk += arraysBFromFile[k][0];
                for (int i = 0; i < arraysBFromFile[k].Length-1; i++)
                {   
                    Hk += arraysBFromFile[k][i+1] * nowVector[i];
                }
                Hks.Add(Hk); Hk = 0;
            }
            int index = Hks.LastIndexOf(Hks.Max());
            OBJ.klas = index; 
            OBJ.outImage = bmp;
            OBJ.Hk = Hks[index];
            OBJ.Show();
            arraysBFromFile = new List<double[]>(); Hks = null; 
        }
        
        public List<double[]> middlesVector(List<double[][]> klasses)
        {
            double summ = 0.0;
            List<double[]> result = new List<double[]>(); double[] res = new double[klasses[0][0].Length];
            foreach (double[][] Vectors in klasses)
            {
                for (int i = 0; i < klasses[0][0].Length; i++)
                {
                    for (int j = 0; j < 360; j++)
                    {
                        summ += Vectors[j][i];
                    }
                    res[i] = summ / Vectors.Length;
                    summ = 0.0;
                }
                result.Add(res);
                res = new double[klasses[0].Length];
            }
            return result;
        }

        public double[] arrayMiddleVector(double[][] Vectors360)
        {
            double summ = 0.0;
            double[] result = new double[Vectors360[0].Length];
            for (int i = 0; i < Vectors360[0].Length; i++)
            {
                for (int j = 0; j < 360; j++)
                {
                    if (Vectors360[j][i] < 1)
                        summ += Vectors360[j][i];
                    else
                        summ += Vectors360[0][i];
                }
                result[i] = summ / 360;
                summ = 0.0;
            }            
            return result;
        }

        public void readMatrixC(int size)
        {
            double[][] vectors = MatrixCreate(size, size);
            string[] arrayFiles;
            if (size == 28)
                arrayFiles = Directory.GetFiles(direct + "\\matrixC\\", "*_Hull.txt");
            else
                arrayFiles = Directory.GetFiles(direct + "\\matrixC\\", "*_Contur.txt");
            for (int k = 0; k < arrayFiles.Length; k++)
            {
                using (StreamReader sr = new StreamReader(arrayFiles[k]))
                {
                    string[] str2 = sr.ReadToEnd().Split(';');
                    for (int i = 0; i < size; i++)
                    {
                        
                        for (int j = 0; j < size; j++)
                        { 
                            vectors[i][j] = Convert.ToDouble(str2[j + (i*size)]);
                        }
                    }
                }
                matrixsCFromFile.Add(vectors);
                vectors = MatrixCreate(size, size);
            }            
        }

        public void readMiddleVectors(int size)
        {
            double[] arrayBk = new double[size + 1];
            string[] arrayFiles;
            if (size == 28)
                arrayFiles = Directory.GetFiles(direct + "\\Middle\\", "*_Hull.txt");
            else
                arrayFiles = Directory.GetFiles(direct + "\\Middle\\", "*_Contur.txt");
            for (int k = 0; k < arrayFiles.Length; k++)
            {
                using (StreamReader sr = new StreamReader(arrayFiles[k]))
                {
                    string[] str = sr.ReadToEnd().Split(';');
                    for (int j = 0; j < str.Length - 1; j++)
                    {
                        arrayBk[j] = Convert.ToDouble(str[j]);
                    }
                }
                middleVectors.Add(arrayBk);
                arrayBk = new double[size + 1];
            }
        }

        public void readArrayB(int size)
        {
            double[] arrayBk = new double[size + 1];
            string[] arrayFiles;
            if (size == 28)
                arrayFiles = Directory.GetFiles(direct + "\\HkHull\\", "*_Hull.txt");
            else
                arrayFiles = Directory.GetFiles(direct + "\\HkContur\\", "*_Contur.txt");
            for (int k = 0; k < arrayFiles.Length; k++)
            {
                using (StreamReader sr = new StreamReader(arrayFiles[k]))
                {
                    string[] str = sr.ReadToEnd().Split(';');
                    for (int j = 0; j < str.Length - 1; j++)
                    {
                        arrayBk[j] = Convert.ToDouble(str[j]);
                    }
                }
                arraysBFromFile.Add(arrayBk);
                arrayBk = new double[size+1];
            }
        }

        public void read360Vectors(int size)
        {
            double[][] vectors = MatrixCreate(360, size);
            string[] arrayFiles;
            if (size == 28)
                arrayFiles = Directory.GetFiles(direct + "\\vectorsHull\\", "*_Hull.txt");
            else
                arrayFiles = Directory.GetFiles(direct + "\\vectorsContur\\", "*_Contur.txt");
            for (int k = 0; k < arrayFiles.Length; k++)
            {
                using (StreamReader sr = new StreamReader(arrayFiles[k]))
                {
                    string[] str = sr.ReadToEnd().Split(':');
                    for (int i = 0; i < 360; i++)
                    {
                        string[] str2 = str[k].Split(';');
                        for (int j = 0; j < str2.Length - 1; j++)
                        {
                            vectors[i][j] = Convert.ToDouble(str2[j]);
                        }
                    }
                }
                vectrosFromFile.Add(vectors);
                vectors = MatrixCreate(360, 28);
            }
            vectors = null;
        }

        public void writeHk(int size, string name)
        {
            double[][] matrixW, matrixA;
            read360Vectors(size);
            string contOrHull;
            if (size == 28)
                contOrHull = "Hull";
            else
                contOrHull = "Contur";
            List<double[]> arrayB = new List<double[]>();
            List<double> elemBk0 = new List<double>();

            List<double[]> midContur = middlesVector(vectrosFromFile);
            matrixW = calcW(vectrosFromFile, midContur, size);
            matrixA = Matrix.PseudoInverse(matrixW);
            for (int k = 0; k < midContur.Count; k++)
            {
                arrayB.Add(calcB(matrixA, midContur[k], size, midContur.Count));
            }
            for (int k = 0; k < midContur.Count; k++)
            {
                elemBk0.Add(calcBk0(arrayB[k], midContur[k], size));
            }
            
            for (int k = 0; k < elemBk0.Count; k++)
            {
                using (StreamWriter sw = new StreamWriter(direct + "\\Hk"+ contOrHull + "\\" + Convert.ToString(k) +
                    "_"+ contOrHull + ".txt", true, System.Text.Encoding.Default))
                {
                    sw.Write(elemBk0[k] + ";");
                    for (int i = 0; i < arrayB[k].Length; i++)
                    {
                        sw.Write(arrayB[k][i] + ";");
                    }
                }
            }
            matrixW = null; matrixA = null;
            arrayB = null; elemBk0 = null; midContur = null;
            vectrosFromFile = new List<double[][]>(); Hks = null;
            arraysBFromFile = new List<double[]>(); middleVectors = new List<double[]>();

        }

        static double[] calcB(double[][] aij, double[] mid, int size, int g)
        {
            double[] result = new double[size];
            double buff = 0;
            int n = 360;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    buff += (aij[i][j] * mid[j]);
                }
                result[i] = (n - g) * buff; buff = 0.0d;
            }
            return result;
        }

        static double calcBk0(double[] bik, double[] mid, int size)
        {
            double result = 0;
            double buff = 0;
            for (int i = 0; i < size; i++)
            {
                buff += (bik[i] * mid[i]);
            }
            result = -buff / 2; buff = 0.0d;
            return result;
        }

        static double[][] calcW(List<double[][]> Vectors, List<double[]> mid, int size)
        {
            double[][] matrixW = MatrixCreate(size, size);
            double xikm = 0, Xik = 0, xjkm = 0, Xjk;
            for (int k = 0; k < Vectors.Count; k++)
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        for (int m = 0; m < 360; m++)
                        {
                            xikm = Vectors[k][m][i];
                            Xik = mid[k][i];
                            xjkm = Vectors[k][m][j];
                            Xjk = mid[k][j];
                            matrixW[i][j] += (xikm - Xik) * (xjkm - Xjk);
                        }
                    }
                }
            }
            return matrixW;
        }
        
        static double[][] MatrixCreate(int rows, int cols)
        {
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[cols];
            return result;
        }
        static string MatrixAsString(double[][] matrix)
        {
            string s = "";
            for (int i = 0; i < matrix.Length; ++i)
            {
                for (int j = 0; j < matrix[i].Length; ++j)
                    s += matrix[i][j].ToString("F9").PadLeft(10) + " ";
                s += Environment.NewLine;
            }
            return s;
        }

    }

}
