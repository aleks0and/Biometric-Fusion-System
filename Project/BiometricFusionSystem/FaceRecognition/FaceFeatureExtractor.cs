using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    //zamien operacje na bitmapach zeby byly na pointerach
    public class FaceFeatureExtractor
    {
        
        int _finalMoment;
        /// <summary>
        /// constructor of the FaceFeatureExtractor class
        /// </summary>
        /// <param name="finalMoment"> the order moment at which the calculation of histogram moment generating function should stop</param>
        public FaceFeatureExtractor(int finalMoment)
        {
            _finalMoment = finalMoment;
        }
        /// <summary>
        /// function which calculates the feature vectors (Gabor function related and Color histogram related) for a given image.
        /// </summary>
        /// <param name="bmp">image for which the function calculates the feature vectors</param>
        /// <returns> list of feature vectors stored as list of double with the following structure
        ///           first the color histogram moments and as last elements gabor filter feature vectors</returns>
        public List<double> GetFeatureVector(Bitmap bmp)
        {
            HistogramEqualization he = new HistogramEqualization();
            bmp = he.Normalize(bmp);
            HistogramFeatureExtraction hfe = new HistogramFeatureExtraction(bmp);
            List<List<double>> histogramFeatures = hfe.CalculateMoments(_finalMoment);
            GrayscaleConverter gConverter = new GrayscaleConverter();
            bmp = gConverter.Normalize(bmp);
            double[] lambda = { 10, 5 };
            GaborFilter gf = new GaborFilter(2, 1, lambda, Math.PI / 2, 4, 3);
            List<Bitmap> gfBitmap = gf.ApplyFilter(bmp);
            bmp = CombineBitmaps(gfBitmap);
            GaborFilterMagnitudes gfm = new GaborFilterMagnitudes(bmp);
            List<double> magnitudesFeatures = gfm.CalculateFeatureVector();
            histogramFeatures.Add(magnitudesFeatures);
            return histogramFeatures.SelectMany(n => n).ToList();
        }
        /// <summary>
        /// function which calculates the feature vectors only for the gabor function for a given image.
        /// </summary>
        /// <param name="bmp">image for which the function calculates the feature vectors</param>
        /// <returns> list of gabor filter feature vectors stored as list of double </returns>
        public List<double> GetGaborFeatureVector(Bitmap bmp)
        {
            HistogramEqualization he = new HistogramEqualization();
            bmp = he.Normalize(bmp);
            GrayscaleConverter gConverter = new GrayscaleConverter();
            bmp = gConverter.Normalize(bmp);
            double[] lambda = { 10, 5 };
            GaborFilter gf = new GaborFilter(2, 1, lambda, Math.PI / 2, 4, 3);
            List<Bitmap> gfBitmap = gf.ApplyFilter(bmp);
            bmp = CombineBitmaps(gfBitmap);
            GaborFilterMagnitudes gfm = new GaborFilterMagnitudes(bmp);
            List<double> magnitudesFeatures = gfm.CalculateFeatureVector();
            return magnitudesFeatures;
        }
        /// <summary>
        /// Function which combines the result of list of pictures into one picture by putting them "on top" of each other
        /// </summary>
        /// <param name="lb">list of bitmaps</param>
        /// <returns> combined image of the input bitmaps</returns>
        public Bitmap CombineBitmaps(List<Bitmap> lb)
        {
            var superpos = new Bitmap(lb[0].Width, lb[0].Height);
            for (int i = 0; i < superpos.Width; i++)
            {
                for (int j = 0; j < superpos.Height; j++)
                {
                    int min = 255;
                    for (int k = 0; k < lb.Count; k++)
                    {
                        var px = lb[k].GetPixel(i, j);
                        if (px.R < min)
                        {
                            min = px.R;
                        }
                    }
                    superpos.SetPixel(i, j, Color.FromArgb(min, min, min));
                }
            }
            return superpos;
        }
    }
}
