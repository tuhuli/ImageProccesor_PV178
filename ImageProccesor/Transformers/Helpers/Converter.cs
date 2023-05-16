using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProccesor.Transformers
{
    public static class Converter
    {

        public static (double, double, double) ConvertRGBToHSI(int red, int green, int blue)
        {
            double r = red / 255.0;
            double g = green / 255.0;
            double b = blue / 255.0;

            double intensity = (r + g + b) / 3.0;

            double minRGB = Math.Min(r, Math.Min(g, b));
            double saturation = 1 - (3 * minRGB / (r + g + b));

            double hue = 0;
            if (saturation != 0)
            {
                double num = 0.5 * ((r - g) + (r - b));
                double den = Math.Sqrt((r - g) * (r - g) + (r - b) * (g - b));
                double theta = Math.Acos(num / den);

                if (b <= g)
                {
                    hue = theta;
                }
                else
                {
                    hue = 2 * Math.PI - theta;
                }
            }

            hue = (hue * 180) / Math.PI;
            if (hue < 0)
            {
                hue += 360;
            }

            return (hue, saturation, intensity);
        }

        public static (int, int, int) ConvertHSIToRGB(double hue, double saturation, double intensity)
        {
            if (saturation == 0)
            {
                int value = (int)(intensity * 255);
                return (value, value, value);
            }

            double h = hue;
            if (h >= 360)
            {
                h -= 360;
            }
            else if (h < 0)
            {
                h += 360;
            }

            double chroma = (1 - Math.Abs((2 * intensity) - 1)) * saturation;
            double x = chroma * (1 - Math.Abs((h / 60) % 2 - 1));
            double m = intensity - chroma / 2;

            double r, g, b;
            if (h < 60)
            {
                r = chroma;
                g = x;
                b = 0;
            }
            else if (h < 120)
            {
                r = x;
                g = chroma;
                b = 0;
            }
            else if (h < 180)
            {
                r = 0;
                g = chroma;
                b = x;
            }
            else if (h < 240)
            {
                r = 0;
                g = x;
                b = chroma;
            }
            else if (h < 300)
            {
                r = x;
                g = 0;
                b = chroma;
            }
            else
            {
                r = chroma;
                g = 0;
                b = x;
            }

            int red = (int)((r + m) * 255);
            int green = (int)((g + m) * 255);
            int blue = (int)((b + m) * 255);

            red = Math.Clamp(red, 0, 255);
            green = Math.Clamp(green, 0, 255);
            blue = Math.Clamp(blue, 0, 255);

            return (red, green, blue);
        }

        public static int GetIntensity(int red, int green, int blue)
        {
            return (red + green + blue) / 3;
        }
    }

    
    


}
