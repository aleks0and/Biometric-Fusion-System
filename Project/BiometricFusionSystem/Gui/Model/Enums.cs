using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Model
{
    public enum AcquisitionMethod
    {
        FromDisk,
        FromCamera
    }

    public enum SilenceRemoval
    {
        Yes,
        No
    }

    public enum VerificationMethod
    {
        FaceOnly,
        SpeechOnly,
        FaceAndSpeech
    }

    public enum IdentificationMethod
    {
        FaceOnly,
        SpeechOnly,
        FaceAndSpeech
    }
}
