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
        //handle exited event on process to know when it ends

        public static void TakePicture()
        {
            Process process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) +
                                    @"\bin\ffmpeg.exe";

            process.StartInfo.Arguments = "-f dshow -i video=\"TOSHIBA Web Camera - HD\" -qscale:v 2 -vframes 1 -s 1280x960 -y \"C:\\Users\\Martyna\\Desktop\\pic.bmp\"";

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
        }

        public static void RecordAudio()
        {
            Process process2 = new Process();
            process2.StartInfo.RedirectStandardOutput = true;
            process2.StartInfo.RedirectStandardError = true;
            process2.StartInfo.FileName = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) +
                                    @"\bin\ffmpeg.exe";

            process2.StartInfo.Arguments = "-f dshow -i audio=\"Internal Mic(IDT High Definition Audio CODEC)\" -t 10 \"C: \\Users\\Martyna\\Desktop\\rec1.wav\"";

            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.CreateNoWindow = true;
            process2.Start();
            Thread.Sleep(12000);
        }
    }
}
