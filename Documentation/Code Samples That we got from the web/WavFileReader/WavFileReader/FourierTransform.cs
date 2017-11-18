using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics;

namespace Common
{
    public class FourierTransform
    {
        //Set length of Fourier?
        public static Complex32[] Apply(IList<short> samples)
        {
            var result = samples.Select(s => new Complex32(s, 0)).ToArray();
            Fourier.Forward(result, FourierOptions.Default);
            return result;
        }
    }
}
