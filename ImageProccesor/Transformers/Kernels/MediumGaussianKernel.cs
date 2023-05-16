using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProccesor.Transformers.Kernels
{
    public class MediumGaussianKernel: Kernel
    {
        public MediumGaussianKernel() : base(new int[,] {
                { 1, 4, 6, 4, 1 },
                { 4, 16, 24, 16, 4 },
                { 6, 24, 36, 24, 6 },
                { 4, 16, 24, 16, 4 },
                { 1, 4, 6, 4, 1 }
            })
        { }
    }
}
