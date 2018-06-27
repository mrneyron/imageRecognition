using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace imageRecognition
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new imgRecForm());
        }
    }
    static class learnOrRecognize
    {
        public static bool Value { get; set; }
    }
    static class hullOrContur
    {
        public static bool Value { get; set; }
    }   
    static class NamesObj
    {
        public static List<string> names = new List<string>();
    }
   
}
