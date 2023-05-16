using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProccesor.Transformers.Kernels
{
    public class Kernel
    {

        public int[,] KernelArray { get; protected set; }
        public int Sum { get; }
        public int Rows {
            get 
            {
                return KernelArray.GetLength(0);
            } 
        }
        public int Columns
        {
            get
            {
                return KernelArray.GetLength(1);
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
       

        public Kernel(int[,] kernelArray)
        {
            KernelArray = kernelArray;

            Sum = ArraySum();
        }

        
        private int ArraySum()
        {
            int sum = 0;
            for (int i = 0; i < KernelArray.GetLength(0); i++)
            {
                for (int j = 0; j < KernelArray.GetLength(1); j++)
                {
                    sum += KernelArray[i, j];
                }
            }
            return sum;
        }
    }
}
