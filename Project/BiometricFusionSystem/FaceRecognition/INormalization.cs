using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    /// <summary>
    /// interface for the function aplying the normalization
    /// </summary>
    public interface INormalization
    {
        Bitmap Normalize(Bitmap bmp);
    }
}
