using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Camera
{
    public class Ffmpeg
    {
        private string _cameraName = "Logitech HD Webcam C270";//"TOSHIBA Web Camera - HD";
        private string _microphoneName = "Microphone (HD Webcam C270)";//"Internal Mic (IDT High Definition Audio CODEC)";
        private Process _process;
        private EventHandler _handler;

        public Ffmpeg()
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;

            if(ci.Name == "pl-PL")
            {
                _microphoneName = "Mikrofon (HD Webcam C270)";
            }
        }

        //handle exited event on process to know when it ends
        /// <summary>
        /// Takes picture using camera
        /// </summary>
        /// <param name="outputPath">name of the output file</param>
        /// <param name="handler">event handler to Exited event</param>
        public void TakePicture(string outputPath, EventHandler handler)
        {
            var path = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            File.Delete(path + "\\" + outputPath);
            _handler = handler;
            _process = new Process();
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.FileName = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) +
                                    @"\ffmpeg.exe";
            _process.StartInfo.Arguments = "-f dshow -i video=\"" + _cameraName + "\" -qscale:v 0 -vframes 1 -s 640x480 -y \"" + outputPath + "\"";
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.CreateNoWindow = true;
            _process.EnableRaisingEvents = true;
            _process.Exited += _handler;
            _process.Start();
        }
        /// <summary>
        /// records a person using camera
        /// before using this function one has to remove the output.wav file if such previously existed
        /// </summary>
        /// <param name="outputPath">name of the output file</param>
        /// <param name="handler">event handler to Exited event</param>
        public void RecordAudio(string outputPath, EventHandler handler)
        {
            var path = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            File.Delete( path + "\\" + outputPath);
            _handler = handler;
            
            _process = new Process();
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.FileName = path + @"\ffmpeg.exe";

            _process.StartInfo.Arguments = "-f dshow -i audio=\"" + _microphoneName + "\" -flags bitexact -ac 1 -ar 44100 -t 3 \"" + outputPath + "\"";

            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.CreateNoWindow = true;
            _process.EnableRaisingEvents = true;
            _process.Exited += _handler;
            _process.Start();
        }
        /// <summary>
        /// Function returning state of the camera process
        /// </summary>
        /// <param name="parameter">always null</param>
        /// <returns>true if picture/recording is still being taken</returns>
        public bool IsBusy (object parameter)
        {
            return _process == null;
        }
        /// <summary>
        /// Function clearing eventhandler, always call after finishing the TakePicture/RecordAudio function
        /// </summary>
        public void EndEvent()
        {
            _process.Exited -= _handler;
            _process = null;
            _handler = null;
        }
    }
}
