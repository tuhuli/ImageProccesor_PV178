using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace ImageProccesor.Transformers
{
    [SupportedOSPlatform("Windows")]
    public static class Resizer
    {
        public static Bitmap FillIn(Bitmap bitmap, int newX, int newY)
        {
            double aspectRatio;
            if (bitmap.Width > bitmap.Height)
            {
                aspectRatio = (double) bitmap.Height / bitmap.Width;
                return new Bitmap(bitmap, new System.Drawing.Size(newX, (int) (newY * aspectRatio)));
            }
            else
            {
                aspectRatio = (double) bitmap.Width / bitmap.Height;
                return new Bitmap(bitmap, new System.Drawing.Size((int)(newX * aspectRatio), newY ));
            }

        }

    }
}
