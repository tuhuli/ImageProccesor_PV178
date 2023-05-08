using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace ImageProccesor.Transformers
{
    public static class LinearFilters
    {
        public static void SmoothingAsync(ImageModel processedImage)
        {
            Bitmap processedBitmap = processedImage.ImageBitmap;
            int[,] gaussianKernel = { {1, 2, 1 },
                                      {2, 4, 2 },
                                      {1, 2, 1 } };
            unsafe
            {
                BitmapData bitmapData = processedBitmap.LockBits(new System.Drawing.Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadWrite, processedBitmap.PixelFormat);
                try
                {
                    

                    int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                    int heightInPixels = bitmapData.Height;
                    int widthInBytes = bitmapData.Width * bytesPerPixel;
                    byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                    Parallel.For(1, heightInPixels - 1, y =>
                    {
                        byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                        for (int x = 1; x < widthInBytes - bytesPerPixel; x += bytesPerPixel)
                        {
                            int[] sum = { 0, 0, 0 };
                            byte* currentPixel = currentLine + x;

                            for (int u = -1; u <= 1; u++)
                            {
                                byte* neighborLine = currentLine + (u * bitmapData.Stride);
                                for (int v = -1; v <= 1; v++)
                                {
                                    byte* neighborPixel = neighborLine + ((x + (v * bytesPerPixel)));
                                    sum[0] += *neighborPixel * gaussianKernel[u + 1, v + 1];
                                    sum[1] += *(neighborPixel + 1) * gaussianKernel[u + 1, v + 1];
                                    sum[2] += *(neighborPixel + 2) * gaussianKernel[u + 1, v + 1];
                                }
                            }
                            currentPixel[0] = (byte)(sum[0] / 16);
                            currentPixel[1] = (byte)(sum[1] / 16);
                            currentPixel[2] = (byte)(sum[2] / 16);
                        }
                    });
                    processedBitmap.UnlockBits(bitmapData);
                }
                catch (Exception ex)
                {
                    Shell.Current.DisplayAlert(" GetSmoothen error", $"{ex.Message}", "click");
                }
                finally { processedBitmap.UnlockBits(bitmapData); }
            }
        }


        public static void AddBrightnessToImage(ImageModel processedImage, int brightnessAddition)
        {
            Bitmap processedBitmap = processedImage.ImageBitmap;
            int originalBlue = processedBitmap.GetPixel(0, 0).B;
            unsafe
            {
                BitmapData bitmapData = processedBitmap.LockBits(new System.Drawing.Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadWrite, processedBitmap.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        byte* currentPixel = currentLine + x;
                        currentPixel[0] = (byte)AddBrightness(currentPixel[0], brightnessAddition);
                        currentPixel[1] = (byte)AddBrightness(currentPixel[1], brightnessAddition);
                        currentPixel[2] = (byte)AddBrightness(currentPixel[2], brightnessAddition);
                    }
                });
                processedBitmap.UnlockBits(bitmapData);
            }
        }

        private static int AddBrightness(int oldValue, int brightnessAddition)
        {
            return oldValue + brightnessAddition < 255 ? oldValue + brightnessAddition : 255;
        }
    }
}
