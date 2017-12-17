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
        private string _cameraName = "TOSHIBA Web Camera - HD";//"Logitech HD Webcam C270";
        private string _microphoneName = "Internal Mic (IDT High Definition Audio CODEC)";//"Mikrofon (HD Webcam C270)";
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

            _process.StartInfo.Arguments = "-f dshow -i video=\"" + _cameraName + "\" -qscale:v 0 -vframes 1 -s 640x480 -y \"" + outputPath + "\"";

            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.CreateNoWindow = true;
            _process.EnableRaisingEvents = true;
            _process.Exited += _handler;
            _process.Start();
            
        }
        /// <summary>
        /// before using this function one has to remove the output.wav file if such previously existed
        /// </summary>
        /// <param name="outputPath"></param>
        /// <param name="handler"></param>
        public void RecordAudio(string outputPath, EventHandler handler)
        {
            var path = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            File.Delete( path + "\\" + outputPath);
            _handler = handler;
            _process = new Process();
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.FileName = path + @"\bin\ffmpeg.exe";

            _process.StartInfo.Arguments = "-f dshow -flags bitexact -i audio=\"" + _microphoneName + "\" -ac 1 -ar 44100 -t 3 \"" + outputPath + "\"";

            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.CreateNoWindow = true;
            _process.EnableRaisingEvents = true;
            _process.Exited += _handler;
            _process.Start();
        }

        public bool IsBusy (object parameter)
        {
            return _process == null;
        }

        public void EndEvent()
        {
            _process.Exited -= _handler;
            _process = null;
            _handler = null;
        }
    }
}
