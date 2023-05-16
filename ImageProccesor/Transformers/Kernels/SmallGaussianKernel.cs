using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProccesor.Transformers.Kernels
{
    public class SmallGaussianKernel: Kernel
    {
        public SmallGaussianKernel() : base(new int[,] {
            { 1, 2, 1 },
            { 2, 4, 2 },
            { 1, 2, 1 }
            })
        { }
        
    }
}
