using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    //zamien operacje na bitmapach zeby byly na pointerach
    class FaceFeatureExtractor
    {
        Bitmap _bmp;
        int _finalMoment;

        public FaceFeatureExtractor(Bitmap bmp, int finalMoment)
        {
            _bmp = bmp;
            _finalMoment = finalMoment;
        }

        public List<List<double>> GetFeatureVector()
        {
            HistogramFeatureExtraction hfe = new HistogramFeatureExtraction(_bmp.Width, _bmp.Height, _bmp);
            List<List<double>> histogramFeatures = hfe.CalculateMoments(_finalMoment);

            double[] lambda = { 10, 5 };
            GaborFilter gf = new GaborFilter(2, 1, lambda, Math.PI / 2, 4, 3);
            List<Bitmap> gfBitmap = gf.ApplyFilter(_bmp);
            _bmp = CombineBitmaps(gfBitmap);

            GaborFilterMagnitudes gfm = new GaborFilterMagnitudes(_bmp);
            List<double> magnitudesFeatures = gfm.CalculateFeatureVector();

            histogramFeatures.Add(magnitudesFeatures);

            return histogramFeatures;
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
