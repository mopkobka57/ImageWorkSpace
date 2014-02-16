﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Imaging;

namespace ImageWorkSpace
{
    public static class Convolution
    {
        public static Bitmap ApplyBright(Bitmap input, double[,] kernel)
        {
            //Получаем байты изображения
            byte[,] inputBytes = BitmapBytes.GetBrightBytes(input);
            byte[,] outputBytes = new byte[input.Width,input.Height];

            int width = input.Width;
            int height = input.Height;
            int kernelWidth = kernel.GetLength(0);
            int kernelHeight = kernel.GetLength(1);

            //Производим вычисления
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double brightSum = 0, kSum = 0;
                    kSum += 0;
                    for (int i = 0; i < kernelWidth; i++)

                        for (int j = 0; j < kernelHeight; j++)
                        {
                            int pixelPosX = x + (i - (kernelWidth / 2));
                            int pixelPosY = y + (j - (kernelHeight / 2));
                            if ((pixelPosX < 0) ||
                              (pixelPosX >= width) ||
                              (pixelPosY < 0) ||
                              (pixelPosY >= height)) continue;
                            byte bright = inputBytes[pixelPosX,pixelPosY];

                            double kernelVal = kernel[i, j];

                            brightSum = bright * kernelVal;

                            kSum += kernelVal;
                        }


                    if (kSum <= 0) kSum = 1;

                    //Контролируем переполнения переменных
                    brightSum /= kSum;
                    if (brightSum < 0) brightSum = 0;
                    if (brightSum > 255) brightSum = 255;

                    //Записываем значения в результирующее изображение
                   // if (x == 200) MessageBox.Show("");
                    outputBytes[x,y] = (byte)brightSum;
                }
            }
            //Возвращаем отфильтрованное изображение
            Bitmap I = BitmapBytes.GetBrightBitmap(outputBytes, width, height);
            // I.Save("C:\\Image\\Image1_new");
            return I;
        }
        public static Bitmap Apply(Bitmap input, double[,] kernel)
        {
            //Получаем байты изображения
            byte[,] inputBytes = BitmapBytes.GetBytes(input);
            byte[,] outputBytes = new byte[input.Width*3,input.Height*3];

            int width = input.Width;
            int height = input.Height;
            int kernelWidth = kernel.GetLength(0);
            int kernelHeight = kernel.GetLength(1);

            //Производим вычисления
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double rSum = 0, gSum = 0, bSum = 0, kSum = 0;

                    for (int i = 0; i < kernelWidth; i++)
                    {
                        for (int j = 0; j < kernelHeight; j++)
                        {
                            int pixelPosX = x + (i - (kernelWidth / 2));
                            int pixelPosY = y + (j - (kernelHeight / 2));
                            if ((pixelPosX < 0) ||
                              (pixelPosX >= width) ||
                              (pixelPosY < 0) ||
                              (pixelPosY >= height)) continue;

                            /*byte r = inputBytes[3 * (width * pixelPosY + pixelPosX) + 0];
                            byte g = inputBytes[3 * (width * pixelPosY + pixelPosX) + 1];
                            byte b = inputBytes[3 * (width * pixelPosY + pixelPosX) + 2];*/
                            byte r = inputBytes[3*pixelPosX+0, 3*pixelPosY+0];
                            byte g = inputBytes[3*pixelPosX+1, 3*pixelPosY+1];
                            byte b = inputBytes[3*pixelPosX+2, 3*pixelPosY+2];

                            double kernelVal = kernel[i, j];

                            rSum += r * kernelVal;
                            gSum += g * kernelVal;
                            bSum += b * kernelVal;

                            kSum += kernelVal;
                        }
                    }

                    if (kSum <= 0) kSum = 1;

                    //Контролируем переполнения переменных
                    rSum /= kSum;
                    if (rSum < 0) rSum = 0;
                    if (rSum > 255) rSum = 255;

                    gSum /= kSum;
                    if (gSum < 0) gSum = 0;
                    if (gSum > 255) gSum = 255;

                    bSum /= kSum;
                    if (bSum < 0) bSum = 0;
                    if (bSum > 255) bSum = 255;

                    //Записываем значения в результирующее изображение
                    outputBytes[3*x+0,3*y+0] = (byte)rSum;
                    outputBytes[3*x+1,3*y+1] = (byte)gSum;
                    outputBytes[3*x+2,3*y+2] = (byte)bSum;
                }
            }
            //Возвращаем отфильтрованное изображение
            return BitmapBytes.GetBitmap(outputBytes, width, height);
            // I.Save("C:\\Image\\Image1_new");
        }
    }
}
