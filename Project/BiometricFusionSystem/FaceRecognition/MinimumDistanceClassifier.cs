using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    public class MinimumDistanceClassifier
    {
        Dictionary<string/*class name*/, List<double>/*mean feature vector for a class*/> _classes;

        public MinimumDistanceClassifier()
        {
            _classes = new Dictionary<string, List<double>>();
        }
        /// <summary>
        /// function adds the class name and mean feature vector for a class to the dictionary
        /// </summary>
        /// <param name="classFV">list of feature vectors that belong to the class className</param>
        /// <param name="className">name of the class</param>
        public void AddToDictionary(List<List<double>> classFV, string className)
        {
            List<double> meanFeatureVector = new List<double>();
            double value = 0;
            for(int i = 0; i < classFV[0].Count; i++)
            {
                for(int j = 0; j < classFV.Count; j++)
                {
                    value += classFV[j][i];
                }
                value /= classFV.Count;
                meanFeatureVector.Add(value);
                value = 0;
            }
            _classes.Add(className, meanFeatureVector);
        }
        /// <summary>
        /// function classifies the input feature vector
        /// </summary>
        /// <param name="fv"></param>
        /// <returns>returns name of the class where the feature vector belongs</returns>
        public string Classify(List<double> fv)
        {
            string className = "";
            double eucDist = 0;
            double minEucDist = double.MaxValue;

            for(int i = 0; i < _classes.Count; i++)
            {
                eucDist = FindEuclideanDistance(_classes.Values.ElementAt(i), fv);
                if (eucDist < minEucDist)
                {
                    minEucDist = eucDist;
                    className = _classes.Keys.ElementAt(i);
                }
            }

            return className;
        }
        /// <summary>
        /// function finds Euclidean distance between two vectors
        /// </summary>
        /// <param name="fv1"></param>
        /// <param name="fv2"></param>
        /// <returns>returns Euclidean distance between two vectors</returns>
        public double FindEuclideanDistance(List<double> fv1, List<double> fv2)
        {
            double value = 0;
            for(int i = 0; i < fv1.Count; i++)
            {
                value += Math.Pow(fv1[i] - fv2[i], 2);
            }
            value = Math.Sqrt(value);
            return value;
        }
        /// <summary>
        /// training function
        /// </summary>
        /// <param name="fvForClass">list of feature vectors belonging to a particular class</param>
        /// <param name="classNames">list of class names</param>
        public void Train(List<List<List<double>>> fvForClass, List<string> classNames)
        {
            for(int i = 0; i < fvForClass.Count; i++)
            {
                AddToDictionary(fvForClass[i], classNames[i]);
            }
        }
    }
}
