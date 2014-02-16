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
    public class BitmapBytes
    {
        public static byte[,] GetBytes(Bitmap Image)
        {
            int height = 3 * Image.Height, width = 3 * Image.Width;
            byte[,] mas = new byte[width, height];
            for (int i=0; i<width; i+=3)
                for (int j = 0; j < height; j+=3)
                {
                    Color C = Image.GetPixel(i/3, j/3);
                    mas[i, j] = C.R;
                    mas[i + 1, j + 1] = C.G;
                    mas[i + 2, j + 2] = C.B;
                }
            return mas;
        }
        public static Bitmap GetBitmap(byte[,] mas, int width, int height)
        {
            Bitmap Image = new Bitmap(width, height);
            for (int i = 0; i < 3 * width; i += 3)
                for (int j = 0; j < 3 * height; j += 3)
                    Image.SetPixel(i/3, j/3, Color.FromArgb(mas[i, j], mas[i + 1, j + 1], mas[i + 2, j + 2]));
            return Image;
        }
        public static Bitmap GetBrightBitmap(byte[,] mas, int width, int height)
        {
            Bitmap Image = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    Image.SetPixel(i, j, Color.FromArgb(mas[i, j], mas[i, j], mas[i, j]));
            return Image;
        }
        public static byte[,] GetBrightBytes(Bitmap Image)
        {
            int width = Image.Width, height = Image.Height;
            byte[,] mas = new byte[width, height];
            for (int i=0; i<width; i++)
                for (int j = 0; j < height; j++)
                {
                    Color C = Image.GetPixel(i, j);
                    mas[i, j] = Convert.ToByte((C.R + C.B + C.G) / 3);
                }
            return mas;
        }
        public static Bitmap GetBlack(Bitmap Image)
        {
            Bitmap I=new Bitmap(Image.Width,Image.Height);
            for (int i = 0; i < Image.Width; i++)
                for (int j = 0; j < Image.Height; j++)
                {
                    Color C = Image.GetPixel(i, j);
                    int col=(C.R+C.B+C.G)/3;
                    I.SetPixel(i, j, ((col>10) ? Color.White : Color.Black));
                }
            return I;
        }
    }
}
