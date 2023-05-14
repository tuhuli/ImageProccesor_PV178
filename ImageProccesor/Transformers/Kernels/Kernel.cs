using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProccesor.Transformers.Kernels
{
    public class Kernel
    {

        public int[,] Array { get; }
        public int Sum { get; }
        public int Rows {
            get 
            {
                return Array.GetLength(0);
            } 
        }
        public int Columns
        {
            get
            {
                return Array.GetLength(1);
            }
        }

        public int RowOffset
        {
            get
            {
                return Rows / 2;
            }
        }

        public int ColumnOffset
        {
            get
            {
                return Columns / 2;
            }
        }
        public static Kernel SmallGaussianKernel { get; }
        public static Kernel MediumGaussianKernel { get; }
        public static Kernel LargeGaussianKernel { get; }
        public static Kernel UnsharpMaskingKernel { get; }

        static Kernel()
        {
            SmallGaussianKernel = new Kernel(new int[,] {
                { 1, 2, 1 },
                { 2, 4, 2 },
                { 1, 2, 1 }
             });

            MediumGaussianKernel = new Kernel(new int[,] {
                { 1, 4, 6, 4, 1 },
                { 4, 16, 24, 16, 4 },
                { 6, 24, 36, 24, 6 },
                { 4, 16, 24, 16, 4 },
                { 1, 4, 6, 4, 1 }
            });

            LargeGaussianKernel = new Kernel(new int[,] {
                { 1, 6, 15, 20, 15, 6, 1 },
                { 6, 36, 90, 120, 90, 36, 6 },
                { 15, 90, 225, 300, 225, 90, 15 },
                { 20, 120, 300, 400, 300, 120, 20 },
                { 15, 90, 225, 300, 225, 90, 15 },
                { 6, 36, 90, 120, 90, 36, 6 },
                { 1, 6, 15, 20, 15, 6, 1 }
            });

            UnsharpMaskingKernel = new Kernel(new int[,] {
                { -1, -1, -1},
                { -1, 9, -1},
                { -1, -1, -1},
            });
        }

        public Kernel(int[,] kernelArray)
        {
            Array = kernelArray;

            Sum = ArraySum();
        }

        
        private int ArraySum()
        {
            int sum = 0;
            for (int i = 0; i < Array.GetLength(0); i++)
            {
                for (int j = 0; j < Array.GetLength(1); j++)
                {
                    sum += Array[i, j];
                }
            }
            return sum;
        }
    }
}
