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
        private double _lambda;
        private double _stdX;
        private double _stdY;
        private int _orientations;
        int _finalMoment;

        public double Lambda
        {
            get
            {
                return _lambda;
            }
        }
        public double StdX
        {
            get
            {
                return _stdX;
            }
        }
        public double StdY
        {
            get
            {
                return _stdY;
            }
        }
        public int Orientations
        {
            get
            {
                return _orientations;
            }
        }
        public int FinalMoment
        {
            get
            {
                return _finalMoment;
            }
        }
        /// <summary>
        /// constructor of the FaceFeatureExtractor class
        /// </summary>
        /// <param name="finalMoment"> the order moment at which the calculation of histogram moment generating function should stop</param>
        public FaceFeatureExtractor(int finalMoment)
        {
            _finalMoment = finalMoment;
            _lambda =  9;
            _stdX = 2;
            _stdY = 1;
            _orientations = 4;
        }
        public FaceFeatureExtractor(int finalMoment, double lambda, double stdX, double stdY, int orientations)
        {
            _finalMoment = finalMoment;
            _lambda = lambda;
            _stdX = stdX;
            _stdY = stdY;
            _orientations = orientations;
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
            
            GaborFilter gf = new GaborFilter(_stdX, _stdY, _lambda, Math.PI / 2, _orientations, 3);
            List<Bitmap> gfBitmap = gf.ApplyFilter(bmp);
            bmp.Dispose();
            bmp = CombineBitmaps(gfBitmap);
            GaborFilterMagnitudes gfm = new GaborFilterMagnitudes(bmp);
            List<double> magnitudesFeatures = gfm.CalculateFeatureVector();
            histogramFeatures.Add(magnitudesFeatures);
            foreach(var b in gfBitmap)
            {
                b.Dispose();
            }
            bmp.Dispose();
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
            
            GaborFilter gf = new GaborFilter(_stdX, _stdY, _lambda, Math.PI / 2, _orientations, 3);
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
            var superpos = new FastBitmap(new Bitmap(lb[0].Width, lb[0].Height));
            var fastBitmaps = lb.Select(b => new FastBitmap(b)).ToList();

            superpos.Start();
            foreach(var fb in fastBitmaps)
            {
                fb.Start();
            }

            for (int i = 0; i < superpos.Width; i++)
            {
                for (int j = 0; j < superpos.Height; j++)
                {
                    int min = 255;
                    for (int k = 0; k < lb.Count; k++)
                    {
                        var px = fastBitmaps[k].GetPixel(i, j);
                        if (px.R < min)
                        {
                            min = px.R;
                        }
                    }
                    superpos.SetPixel(i, j, Color.FromArgb(min, min, min));
                }
            }

            foreach(var fb in fastBitmaps)
            {
                fb.End();
            }
            superpos.End();
            return superpos.Bmp;
        }
    }
}
