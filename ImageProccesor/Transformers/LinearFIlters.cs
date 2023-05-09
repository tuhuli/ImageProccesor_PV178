using ImageProccesor.Transformers.Kernels;
using SixLabors.ImageSharp.Processing.Processors.Convolution;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace ImageProccesor.Transformers
{
    [SupportedOSPlatform("Windows")]
    public static class LinearFilters
    {
        public static async Task SmoothingAsync(ImageModel processedImage)
        {
            Bitmap processedBitmap = processedImage.ImageBitmap;
            Kernel kernel = Kernel.LargeGaussianKernel;
            processedImage.ImageBitmap = Convolve(processedBitmap, kernel);
            

        }



        private static Bitmap Convolve(Bitmap processedBitmap, Kernel kernel)
        {
            Bitmap newBitmap = (Bitmap)processedBitmap.Clone();

            unsafe
            {
                BitmapData bitmapData = processedBitmap.LockBits(
                    new System.Drawing.Rectangle(0,0, processedBitmap.Width, processedBitmap.Height),
                    ImageLockMode.ReadWrite,
                    processedBitmap.PixelFormat
                    );

                BitmapData newBitmapData = newBitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, newBitmap.Width, newBitmap.Height),
                    ImageLockMode.ReadWrite,
                    newBitmap.PixelFormat
                    );


                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;
                byte* PtrNewPixel = (byte*)newBitmapData.Scan0;

                Parallel.For(1, bitmapData.Height - 1, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    byte* newCurrentLine = PtrNewPixel + (y * newBitmapData.Stride);
                    for (int x = bytesPerPixel; x < widthInBytes - bytesPerPixel; x += bytesPerPixel)
                    {
                        int[] sum = { 0, 0, 0 };
                        byte* currentPixel = currentLine + x;
                        byte* newCurrentPixel = newCurrentLine + x;

                        for (int u = -kernel.Rows / 2; u <= kernel.Rows / 2; u++)
                        {
                            byte* neighborLine = currentLine + (u * bitmapData.Stride);
                            for (int v = -kernel.Columns / 2; v <= kernel.Columns / 2; v++)
                            {
                                byte* neighborPixel = neighborLine + ((x + (v * bytesPerPixel)));
                                sum[0] += *neighborPixel * kernel.Array[u + kernel.Rows / 2, v + kernel.Columns / 2];
                                sum[1] += *(neighborPixel + 1) * kernel.Array[u + kernel.Rows / 2, v + kernel.Columns / 2];
                                sum[2] += *(neighborPixel + 2) * kernel.Array[u + kernel.Rows / 2, v + kernel.Columns / 2];
                            }
                        }
                        newCurrentPixel[0] = (byte)(sum[0] / kernel.Sum);
                        newCurrentPixel[1] = (byte)(sum[1] / kernel.Sum);
                        newCurrentPixel[2] = (byte)(sum[2] / kernel.Sum);
                        

                    }
                    
                });
                processedBitmap.UnlockBits(bitmapData);
                newBitmap.UnlockBits(newBitmapData);
                return newBitmap;

            }
            
        }


        public static async Task AddBrightnessToImageAsync(ImageModel processedImage, int brightnessAddition)
        {
            await Task.Run(() =>
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
                
            });
        }

        private static int AddBrightness(int oldValue, int brightnessAddition)
        {
            return oldValue + brightnessAddition < 255 ? oldValue + brightnessAddition : 255;
        }
    }
}
