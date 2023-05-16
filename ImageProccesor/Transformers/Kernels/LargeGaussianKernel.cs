using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProccesor.Transformers.Kernels
{
    public class LargeGaussianKernel: Kernel
    {
        public LargeGaussianKernel(): base(new int[,] {
                { 1, 6, 15, 20, 15, 6, 1 },
                { 6, 36, 90, 120, 90, 36, 6 },
                { 15, 90, 225, 300, 225, 90, 15 },
                { 20, 120, 300, 400, 300, 120, 20 },
                { 15, 90, 225, 300, 225, 90, 15 },
                { 6, 36, 90, 120, 90, 36, 6 },
                { 1, 6, 15, 20, 15, 6, 1 }
            })
        { }
    }
}
