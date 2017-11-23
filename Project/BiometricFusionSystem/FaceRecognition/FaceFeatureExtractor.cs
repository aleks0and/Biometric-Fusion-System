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

        public FaceFeatureExtractor(int finalMoment)
        {
            
            _finalMoment = finalMoment;
        }

        public List<double> GetFeatureVector(Bitmap bmp)
        {
            HistogramEqualization he = new HistogramEqualization();
            bmp = he.Normalize(bmp);
            HistogramFeatureExtraction hfe = new HistogramFeatureExtraction(bmp.Width, bmp.Height, bmp);
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
