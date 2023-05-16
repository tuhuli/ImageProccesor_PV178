using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProccesor.Transformers.Kernels
{
    class UnsharpMaskingKernel:Kernel
    {
        public UnsharpMaskingKernel() : base(new int[,] {
                {-1,-1,-1},
                {-1, 9,-1},
                { 1,-1,-1}
            })
        { }
    }
}
