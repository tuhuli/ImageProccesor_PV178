
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;


namespace ImageProccesor.Transformers.Histograms
{
    [SupportedOSPlatform("Windows")]
    public class Histogram
    {
        private int[] _intensities;
        private int[] _cumulativeHistogram;
        private readonly object lockObject = new();
        private readonly Bitmap _bitmap;
        public Histogram(Bitmap bitmap)
        {
            _bitmap = bitmap;
            FillHistogram();
            CalculateCumulativeHistogram();
        }


        

        private void FillHistogram()
        {
            _intensities = new int[256];


            unsafe
            {
                BitmapData bitmapData = _bitmap.LockBits(
                    new System.Drawing.Rectangle(
                        0,
                        0,
                        _bitmap.Width,
                        _bitmap.Height),
                    ImageLockMode.ReadWrite,
                    _bitmap.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(_bitmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        byte* currentPixel = currentLine + x;
                        AddPixelToHistogram(currentPixel[2], currentPixel[1], currentPixel[0]);
                    }
                });
                _bitmap.UnlockBits(bitmapData);
            }
        }

        private void AddPixelToHistogram(int red, int green, int blue)
        {
            int intensity = Converter.GetIntensity(red, green, blue);
            lock (lockObject)
            {
                _intensities[intensity]++;
            }
        }

        private void CalculateCumulativeHistogram()
        {
            _cumulativeHistogram = new int[256];
            _cumulativeHistogram[0] = _intensities[0];


            for (int i = 1; i < 256; i++)
            {
                _cumulativeHistogram[i] = _cumulativeHistogram[i - 1] + _intensities[i];
            }

        }

        public void NormalizeCumulativeHistogram()
        {
            int totalpixels = _bitmap.Height * _bitmap.Width;
            for (int i = 0; i < 256; i++)
            {
                _cumulativeHistogram[i] = (int)Math.Round((double)_cumulativeHistogram[i] * 255 / totalpixels);
            }
        }

        public void ApplyEqualization()
        {

            unsafe
            {
                BitmapData bitmapData = _bitmap.LockBits(
                    new System.Drawing.Rectangle(
                        0,
                        0,
                        _bitmap.Width,
                        _bitmap.Height),
                    ImageLockMode.ReadWrite,
                    _bitmap.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(_bitmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        byte* currentPixel = currentLine + x;
                        (currentPixel[2], currentPixel[1], currentPixel[0]) = GetEqualizedPixel(currentPixel[2],
                            currentPixel[1],
                            currentPixel[0]);

                    }
                });
                _bitmap.UnlockBits(bitmapData);
            }


        }

        private (byte, byte, byte) GetEqualizedPixel(int red, int green, int blue)
        {
            double intensity, saturation, hue;
            (intensity, saturation, hue) = Converter.ConvertRGBToHSI(red, green, blue);
            int newIntensity = Converter.GetIntensity(red, green, blue);
            int equalizedIntensity = _cumulativeHistogram[newIntensity];
            int maxIntensity = _cumulativeHistogram[255];


            int normalizedIntensity = (int)Math.Round((double)equalizedIntensity * 255 / maxIntensity);

            (int newRed, int newGreen, int newBlue) = Converter.ConvertHSIToRGB(hue, saturation, normalizedIntensity);

            return ((byte)newRed, (byte)newGreen, (byte)newBlue);
        }
    }
}


