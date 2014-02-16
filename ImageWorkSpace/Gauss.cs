using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Imaging;

namespace ImageWorkSpace
{
    class Gauss
    {
        public static double[,] Gaussian(int sigma)
        {
            double[,] mas = new double[sigma, sigma];
            int x0, y0;
            x0 = y0 = sigma / 2 + 1;
            double GaussKoef = 1 / (Math.Sqrt(Math.PI * 2) * sigma);
            for (int i = 0; i < sigma; i++)
                for (int j = 0; j < sigma; j++)
                    mas[i, j] = GaussKoef * Math.Exp(-Math.Pow((i - x0 + j - y0) / sigma, 2) * 0.5);
            return mas;
        }
        public static Bitmap MakeGauss(Bitmap image, int sigma)
        {
            Bitmap img = Convolution.Apply(image, Gaussian(sigma));
            return img;
        }
        public static Bitmap SetScale(Bitmap image, int k)
        {
            int width = image.Width, height = image.Height, scale = (int)Math.Pow(2, k);
            Bitmap img = new Bitmap(width / k, height / k);
            byte[,] mas = BitmapBytes.GetBrightBytes(image);
            for (int i = 0; i < width; i += scale)
                for (int j = 0; j < height; j += scale)
                {
                    int col = mas[i, j];
                    img.SetPixel(i / scale, j / scale, Color.FromArgb(col,col,col));
                }
            return img;
        }
    }
}
