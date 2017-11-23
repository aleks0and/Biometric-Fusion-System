using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DynamicTimeWarping : IVerifier
    {
        private double _threshold;

        public DynamicTimeWarping(double threshold)
        {
            _threshold = threshold;
        }

        public double GetDistance(double a, double b)
        {
            return Math.Abs(a - b);
        }

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

        public bool Verify(List<double> input, List<double> template)
        {
            double distance = Compare(input, template);

            double total = template.Sum();
            return (distance / total) < _threshold;
        }
    }
}
