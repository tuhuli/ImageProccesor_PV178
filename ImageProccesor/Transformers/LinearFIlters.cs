using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace ImageProccesor.Transformers
{
    public static class LinearFilters
    {
        public static void Smoothing(ImageModel imageModel)
        {
            Bitmap original_bitmap = imageModel.ImageBitmap;
            Bitmap bitmap = (Bitmap) original_bitmap.Clone();
            int[,] gaussianKernel = { {1, 2, 1 },
                                      {2, 4, 2 },
                                      {1, 2, 1 } };
            int width = bitmap.Width;
            int height = bitmap.Height;

            Parallel.For(1, width, x =>
            {
                for (int y = 1; y < height; y++)
                {
                    bitmap.SetPixel(x, y, GetConvolutionValue(bitmap, x, y, gaussianKernel));
                }
            });

            imageModel.ImageBitmap = bitmap;

        }

        public static Color GetConvolutionValue(Bitmap image, int x, int y, int[,] kernel)
        {
            int[] sum = { 0, 0, 0 }; 
            for (int u = -1; u <= 1; u++)
            {
                for (int v = -1; v <= 1; v++)
                {
                    Color value = image.GetPixel(x + u, y + v);
                    sum[0] += value.R * kernel[u + 1,v + 1];
                    sum[1] += value.G * kernel[u + 1, v + 1];
                    sum[2] += value.B * kernel[u + 1, v + 1];

                }
            }
            return Color.FromArgb(sum[0] / 16, sum[1] / 16, sum[2] / 16);
        }
    }
}
