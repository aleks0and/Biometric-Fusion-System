using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Class responsible for calculation of DTW
    /// </summary>
    public class DynamicTimeWarping : IVerifier
    {
        private double _threshold;
        /// <summary>
        /// setting the value of the threshold
        /// </summary>
        /// <param name="threshold"> vlaue which sets the limit at which it decides whether two files are similar or not</param>
        public DynamicTimeWarping(double threshold)
        {
            _threshold = threshold;
        }
        /// <summary>
        /// function which finds the absolute difference bewtween two numbers
        /// </summary>
        public double GetDistance(double a, double b)
        {
            return Math.Abs(a - b);
        }
        /// <summary>
        /// algorithm calculating the DTW which measures the similarities between two sequences.
        /// </summary>
        /// <param name="input"> input values which are to be compared to the template </param>
        /// <param name="template"> template values </param>
        /// <returns> returns the sum of differences between the tested sample - input and template </returns>
        public double Compare(List<double> input, List<double> template)
        {
            var distances = new double[input.Count + 1, template.Count + 1];
            for(int i = 0; i < input.Count + 1; i++)
            {
                distances[i, 0] = double.MaxValue;
            }
            for(int i = 0; i < template.Count + 1; i++)
            {
                distances[0, i] = double.MaxValue;
            }
            distances[0, 0] = 0;

            for(int i = 1; i < input.Count + 1; i++)
            {
                for(int j = 1; j < template.Count + 1; j++)
                {
                    distances[i, j] = GetDistance(input[i - 1], template[j - 1]) +
                        Math.Min(distances[i - 1, j], Math.Min(distances[i, j - 1],
                        distances[i - 1, j - 1]));
                }
            }

            return distances[input.Count, template.Count];
        }
        /// <summary>
        /// Functions verifies whether two samples have satisfactory similarities in order to state that they are of same speaker. 
        /// </summary>
        /// <param name="input"> input values which are to be compared to the template </param>
        /// <param name="template"> template values </param>
        /// <returns> true if the ratio of differences and template values is below the set threshold.</returns>
        public bool Verify(List<double> input, List<double> template)
        {
            double distance = Compare(input, template);
            double total = template.Sum();
            return (distance / total) < _threshold;
        }
    }
}
