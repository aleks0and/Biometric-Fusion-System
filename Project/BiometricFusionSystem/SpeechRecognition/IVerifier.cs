using System.Collections.Generic;

namespace SpeechRecognition
{
    public interface IVerifier
    {
        bool Verify(List<double> input, List<double> template);
    }
}