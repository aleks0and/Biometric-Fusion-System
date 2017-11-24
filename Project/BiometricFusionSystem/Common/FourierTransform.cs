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
        /// <summary>
        /// Function which applies Fourier Transform to a given data
        /// </summary>
        /// <param name="samples">List of short numbers representing data to which FT is to be applied</param>
        /// <returns>result of FT over the given data</returns>
        public static Complex32[] Apply(IList<short> samples)
        {
            var result = samples.Select(s => new Complex32(s, 0)).ToArray();
            Fourier.Forward(result, FourierOptions.Default);
            return result;
        }
    }
}
