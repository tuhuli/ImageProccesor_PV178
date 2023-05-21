using ImageProccesor.Transformers.Kernels;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;



namespace ImageProccesor.Transformers
{
    [SupportedOSPlatform("Windows")]
    public static class LinearFilters
    {
        public static async Task SmoothingAsync(ImageModel processedImage, Kernel kernel)
        {
            await Task.Run(() =>
            {
                Bitmap processedBitmap = processedImage.ImageBitmap;
                processedImage.ImageBitmap = LinearFiltersHelper.Convolve(processedBitmap, kernel);
                processedBitmap.Dispose();
            });

        }


        public static async Task AddBrightnessToImageAsync(ImageModel processedImage, int brightnessAddition)
        {
            await Task.Run(() =>
            {
                Bitmap processedBitmap = processedImage.ImageBitmap;
                unsafe
                {
                    BitmapData bitmapData = processedBitmap.LockBits(
                        new System.Drawing.Rectangle(
                            0, 
                            0, 
                            processedBitmap.Width,
                            processedBitmap.Height),
                        ImageLockMode.ReadWrite,
                        processedBitmap.PixelFormat);

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
                            currentPixel[0] = (byte)LinearFiltersHelper.AddBrightness(currentPixel[0], brightnessAddition);
                            currentPixel[1] = (byte)LinearFiltersHelper.AddBrightness(currentPixel[1], brightnessAddition);
                            currentPixel[2] = (byte)LinearFiltersHelper.AddBrightness(currentPixel[2], brightnessAddition);
                        }
                    });
                    processedBitmap.UnlockBits(bitmapData);
                }      
            });
        }

        public static async Task ModifyHueAsync(ImageModel processedImage, int hue)
        {
            await Task.Run(() =>
            {
                Bitmap processedBitmap = processedImage.ImageBitmap;
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
                            int r, g, b;
                            (r, g, b) = LinearFiltersHelper.AddHue(currentPixel[0], currentPixel[1], currentPixel[2], hue);

                            currentPixel[0] = (byte) b;
                            currentPixel[1] = (byte) g;
                            currentPixel[2] = (byte) r;
                        }
                    });
                    processedBitmap.UnlockBits(bitmapData);
                }
            });
        }

        public static async Task  HistogramEqalizationAsync(ImageModel processedImage)
        {
            await Task.Run(() =>
            {
                Histogram histogram = new(processedImage.ImageBitmap);
                histogram.NormalizeCumulativeHistogram();
                histogram.ApplyEqualization();
            });
        }

    }
}
