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
        private Dictionary<string, List<double>> _classes;
        public Dictionary<string, List<double>> Classes
        {
            get { return _classes; }
        }
        /// <summary>
        /// setting the value of the threshold
        /// </summary>
        /// <param name="threshold"> vlaue which sets the limit at which it decides whether two files are similar or not</param>
        public DynamicTimeWarping(double threshold)
        {
            _threshold = threshold;
            _classes = new Dictionary<string, List<double>>();
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
        /// function adds the class name and mean feature vector for a class to the dictionary
        /// </summary>
        /// <param name="classFV">list of feature vectors that belong to the class className</param>
        /// <param name="className">name of the class</param>
        public void AddToDictionary(List<List<double>> classFV, string className)
        {
            double minimalAverage = double.MaxValue;
            int index = 0;
            for (int i=0; i<classFV.Count; i++)
            {
                double average = 0; 
                for(int j=0; j<classFV.Count; j++)
                {
                    if (j!=i)
                    {
                        average += Compare(classFV[i], classFV[j]);
                    }
                }
                average /= (classFV.Count - 1);
                if (minimalAverage > average)
                {
                    minimalAverage = average;
                    index = i;
                }
            }
            _classes.Add(className,classFV[index]);
        }
        /// <summary>
        /// training function
        /// </summary>
        /// <param name="fvForClass">list of feature vectors belonging to a particular class</param>
        /// <param name="classNames">list of class names</param>
        public void Train(List<List<List<double>>> fvForClass, List<string> classNames)
        {
            for (int i = 0; i < fvForClass.Count; i++)
            {
                AddToDictionary(fvForClass[i], classNames[i]);
            }
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
        /// <summary>
        /// function classifies the input feature vector
        /// </summary>
        /// <param name="input"></param>
        /// <returns>returns name of the class where the feature vector belongs</returns>
        public string Classify(List<double> input)
        {
            string className = "";
            double dist = 0;
            double minDist = double.MaxValue;

            for (int i = 0; i < _classes.Count; i++)
            {
                dist = Compare(input, _classes.Values.ElementAt(i));
                if (dist < minDist)
                {
                    minDist = dist;
                    className = _classes.Keys.ElementAt(i);
                }
            }

            return className;
        }
    }
}
