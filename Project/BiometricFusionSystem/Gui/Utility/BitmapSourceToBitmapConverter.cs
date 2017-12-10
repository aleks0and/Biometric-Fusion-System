using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Gui.Utility
{
    public class BitmapSourceToBitmapConverter
    {
        public static Bitmap Convert(BitmapImage image)
        {
            using (var stream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(stream);
                return new Bitmap(stream);
            }
        }
    }
}
