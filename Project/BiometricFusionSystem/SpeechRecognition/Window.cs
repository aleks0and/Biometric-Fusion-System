using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition
{
    /// <summary>
    /// interface for the window functions
    /// </summary>
    public interface Window
    {
        void ApplyWindow(Frame frame);
    }
}
