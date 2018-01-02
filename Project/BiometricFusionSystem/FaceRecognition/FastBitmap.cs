using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    public unsafe class FastBitmap : IDisposable
    {
        private Bitmap _bmp;
        private Rectangle _rect;
        private BitmapData _data;

        public int Width
        {
            get { return _bmp.Width; }
        }
        public int Height
        {
            get { return _bmp.Height; }
        }
        public Bitmap Bmp
        {
            get { return _bmp; }
        }
        public FastBitmap(Bitmap bmp)
        {
            _bmp = bmp;
            _rect = new Rectangle(Point.Empty, bmp.Size);
        }

        public void Start()
        {
            if (_data == null)
            {
                _data = _bmp.LockBits(_rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            }
        }
        public void End()
        {
            if (_data != null)
            {
                _bmp.UnlockBits(_data);
                _data = null;
            }
        }
        public void Dispose()
        {
            _bmp.Dispose();
        }

        public unsafe Color GetPixel(int x, int y)
        {
            Byte* img = (Byte*)_data.Scan0.ToPointer();
            var offset = _data.Stride * y + x * 3;
            var b = img[offset];
            var g = img[offset + 1];
            var r = img[offset + 2];
            return Color.FromArgb(r, g, b);
        }

        public unsafe void SetPixel(int x, int y, Color c)
        {
            Byte* img = (Byte*)_data.Scan0.ToPointer();
            var offset = _data.Stride * y + x * 3;
            img[offset] = c.B;
            img[offset + 1] = c.G;
            img[offset + 2] = c.R;
        }
    }
}
