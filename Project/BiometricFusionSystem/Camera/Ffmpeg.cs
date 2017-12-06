using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camera
{
    public class Ffmpeg
    {
        private string _cameraName = "Logitech HD Webcam C270";
        private string _microphoneName = "Mikrofon(HD Webcam C270)";
        private Process _process;
        private EventHandler _handler;
        //handle exited event on process to know when it ends

        public void TakePicture(string outputPath, EventHandler handler)
        {
            _handler = handler;
            _process = new Process();
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.FileName = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) +
                                    @"\bin\ffmpeg.exe";

            _process.StartInfo.Arguments = "-f dshow -i video=\"" + _cameraName + "\" -qscale:v 2 -vframes 1 -s 1280x960 -y \"" + outputPath + "\"";

            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.CreateNoWindow = true;
            _process.EnableRaisingEvents = true;
            _process.Exited += _handler;
            _process.Start();
            
        }

        public void RecordAudio(string outputPath, EventHandler handler)
        {
            _handler = handler;
            _process = new Process();
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.FileName = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) +
                                    @"\bin\ffmpeg.exe";

            _process.StartInfo.Arguments = "-f dshow -i audio=\"" + _microphoneName + "\" -t 10 \"" + outputPath + "\"";

            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.CreateNoWindow = true;
            _process.EnableRaisingEvents = true;
            _process.Exited += _handler;
            _process.Start();
            Thread.Sleep(12000);
        }

        public void EndEvent()
        {
            _process.Exited -= _handler;
            _process = null;
            _handler = null;
        }
    }
}
