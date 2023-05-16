using ImageProccesor.Transformers.Kernels;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.Versioning;

namespace ImageProccesor.Transformers
{
    [SupportedOSPlatform("Windows")]
    public static class LinearFiltersHelper
    {
        public static (int, int, int) AddHue(int b, int g, int r, int hue)
        {
            double h, s, i;
            (h, s, i) = Converter.ConvertRGBToHSI(r, g, b);
            h += hue;
            if (h >= 360)
            {
                h -= 360;
            }
            else if (h < 0)
            {
                h += 360;

            }
            return Converter.ConvertHSIToRGB(h, s, i);

        }

        public static Bitmap Convolve(Bitmap processedBitmap, Kernel kernel)
        {
            Bitmap newBitmap = (Bitmap)processedBitmap.Clone();



            unsafe
            {
                BitmapData bitmapData = processedBitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height),
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

                Parallel.For(kernel.RowOffset, bitmapData.Height - kernel.RowOffset, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    byte* newCurrentLine = PtrNewPixel + (y * newBitmapData.Stride);
                    for (int x = bytesPerPixel * kernel.ColumnOffset;
                         x < widthInBytes - bytesPerPixel * kernel.ColumnOffset;
                         x += bytesPerPixel)
                    {
                        int[] sum = { 0, 0, 0 };
                        byte* currentPixel = currentLine + x;
                        byte* newCurrentPixel = newCurrentLine + x;

                        for (int u = -kernel.RowOffset; u <= kernel.RowOffset; u++)
                        {
                            byte* neighborLine = currentLine + (u * bitmapData.Stride);
                            for (int v = -kernel.ColumnOffset; v <= kernel.ColumnOffset; v++)
                            {
                                byte* neighborPixel = neighborLine + ((x + (v * bytesPerPixel)));
                                sum[0] += *neighborPixel * kernel.KernelArray[u + kernel.RowOffset, v + kernel.ColumnOffset];
                                sum[1] += *(neighborPixel + 1) * kernel.KernelArray[u + kernel.RowOffset, v + kernel.ColumnOffset];
                                sum[2] += *(neighborPixel + 2) * kernel.KernelArray[u + kernel.RowOffset, v + kernel.ColumnOffset];

                            }
                        }
                        newCurrentPixel[0] = (byte) Math.Clamp((sum[0] / kernel.Sum), 0, 255);
                        newCurrentPixel[1] = (byte) Math.Clamp((sum[1] / kernel.Sum), 0, 255);
                        newCurrentPixel[2] = (byte) Math.Clamp((sum[2] / kernel.Sum), 0, 255);


                    }

                });
                processedBitmap.UnlockBits(bitmapData);
                newBitmap.UnlockBits(newBitmapData);

                return newBitmap;

            }

        }

        public static int AddBrightness(int oldValue, int brightnessAddition)
        {
            if (brightnessAddition >= 0)
            {
                return oldValue + brightnessAddition < 255 ? oldValue + brightnessAddition : 255;
            }
            else
            {
                return oldValue + brightnessAddition > 0 ? oldValue + brightnessAddition : 0;
            }
        }
    }
}
