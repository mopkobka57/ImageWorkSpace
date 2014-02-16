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
    class ImageFilter
    {
        private string path = "";
        private const int MedN = 3, MedN2 = MedN * MedN, KerN = 3, KerN2 = KerN * KerN, BLUE = 255, GREEN = 256 * BLUE, RED = 256 * GREEN;
        public Bitmap Image;
        private int[,] img;
        private double[,] kernel;
        private int Height, Width, offset;
        public double div;
        public ImageFilter(string s)
        {
            path = s;
            Image = new Bitmap(s);
            Height = Image.Width;
            Width = Image.Height;
            img = new int[Height + 2, Width + 2];
            Color C;
            for (int i = 1; i < Height + 1; i++)
                for (int j = 1; j < Width + 1; j++)
                {
                    C = Image.GetPixel(i - 1, j - 1);
                    img[i, j] = (C.R * 16 * 16 + C.G) * 16 * 16 + C.B;
                }
            img[0, 0] = img[1, 1];
            img[0, Width + 1] = img[1, Width];
            img[Height + 1, 0] = img[Height, 1];
            img[Height + 1, Width + 1] = img[Height, Width];
            for (int i = 1; i < Width + 1; i++)
            {
                img[0, i] = img[1, i];
                img[Height + 1, i] = img[Height, i];
            }
            for (int i = 1; i < Height + 1; i++)
            {
                img[i, 0] = img[i, 1];
                img[i, Width + 1] = img[i, Width];
            }
            kernel = new double[KerN, KerN];
        }
        public void Blur()
        {
            kernel[0, 0] = 1;
            kernel[0, 1] = 1;
            kernel[0, 2] = 1;
            kernel[1, 0] = 1;
            kernel[1, 1] = 1;
            kernel[1, 2] = 1;
            kernel[2, 0] = 1;
            kernel[2, 1] = 1;
            kernel[2, 2] = 1;
            Image = Convolution.Apply(this.Image, kernel);
            return;
            Div();
            Multiple();
        }
        public void Negative()
        {
            kernel[0, 0] = -1;
            kernel[1, 0] = -2;
            kernel[2, 0] = -1;
            kernel[0, 1] = 0;
            kernel[1, 1] = 0;
            kernel[2, 1] = 0;
            kernel[0, 2] = 1;
            kernel[1, 2] = 2;
            kernel[2, 2] = 1;
            Image = Convolution.Apply(this.Image, kernel);
            return;
            Div();
            Multiple();
        }
        public void Clarity()
        {
            kernel[0, 0] = -1;
            kernel[1, 0] = -1;
            kernel[2, 0] = -1;
            kernel[0, 1] = -1;
            kernel[1, 1] = 9;
            kernel[2, 1] = -1;
            kernel[0, 2] = -1;
            kernel[1, 2] = -1;
            kernel[2, 2] = -1;
            Image = Convolution.Apply(this.Image, kernel);
            return;
            Div();
            Multiple();
        }
        private void Div()
        {
            div = 0;
            offset = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    //div += ((kernel[i, j]>0) ? kernel[i,j] : -kernel[i,j]);
                    div += kernel[i, j];
                    if (kernel[i, j] < 0) offset = 255;
                }
            if (div < 0)
            {
                div = -div;
                //offset = 255;
            }
            if (div == 0) div = 1;
            //div = -1;
            //div = 3;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    kernel[i, j] /= div;
            /*offset = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    div += kernel[i, j];
                    if (kernel[i, j] < 0) offset = 255;
                }*/
        }
        private void Multiple()
        {
            for (int i = 1; i < Height + 1; i++)
                for (int j = 1; j < Width + 1; j++)
                {
                    double rdf = 0, grf = 0, blf = 0;
                    for (int a = 0; a < 3; a++)
                        for (int b = 0; b < 3; b++)
                        {
                            rdf += ((img[i - 1 + a, j - 1 + b] & RED) >> 16) * kernel[a, b];
                            grf += ((img[i - 1 + a, j - 1 + b] & GREEN) >> 8) * kernel[a, b];
                            blf += (img[i - 1 + a, j - 1 + b] & BLUE) * kernel[a, b];
                        }
                    int rd = (int)Math.Round(rdf), gr = (int)Math.Round(grf), bl = (int)Math.Round(blf);
                    if (rd <= 0)
                        rd += offset;
                    if (gr <= 0)
                        gr += offset;
                    if (bl <= 0)
                        bl += offset;
                    /*if (rd > 255) rd = 255;
                    if (gr > 255) gr = 255;
                    if (bl > 255) bl = 255;*/
                    rd <<= 16;
                    gr <<= 8;
                    /*rd &= RED;
                    gr &= GREEN;
                    bl &= BLUE;*/
                    img[i, j] = (rd + gr + bl);
                }
            for (int i = 1; i < Height + 1; i++)
                for (int j = 1; j < Width + 1; j++)
                {
                    Image.SetPixel(j - 1, i - 1, Color.FromArgb(img[i, j]));
                }

        }
        private int Round(float a)
        {
            return (int)Math.Round(a);
        }
        public static Bitmap Median(Bitmap Image)
        {
            int width = Image.Width;
            int height = Image.Height;
            Bitmap I = new Bitmap(width, height);
            /*int[] mas = new int[MedN2];
            for (int i = 1; i < Height + 1; i++)
                for (int j = 1; j < Width + 1; j++)
                {
                    for (int k = 0; k < MedN2; k++)
                        mas[k] = img[i + k / MedN - 1, j + k % MedN - 1];

                    Array.Sort(mas);
                    img[i, j] = mas[MedN2 / 2];
                }*/
            int[,] mas = new int[width, height];
            int[,] img = new int[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    Color C = Image.GetPixel(i, j);
                    img[i, j] = (C.R * 256 + C.G) * 256 + C.B;
                }
            for (int i=1; i<width-1; i++)
                for (int j = 1; j < height - 1; j++)
                {
                    int[] arr = new int[9];
                    int k = 0;
                    for (int a = 0; a < 3; a++)
                        for (int b = 0; b < 3; b++, k++)
                            arr[k] = img[i + a - 1, j + b - 1];
                    Array.Sort(arr);
                    mas[i, j] = arr[4];
                }
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    I.SetPixel(i, j, Color.FromArgb(255,Color.FromArgb(mas[i,j])));
                }
            I.Save("C:\\Image\\Cveti.jpg");
            return I;
        }
        public void BinarBorders()
        {
            int[, ,] mas = new int[24, Height + 2, Width + 2];
        }
        public void Save(string s)
        {
            try
            {
                Image.Save(s);
            }
            catch
            {
                MessageBox.Show("Выберите другое имя");
            }
        }
    }
}
