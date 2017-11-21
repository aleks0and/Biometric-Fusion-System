using System.Collections.Generic;

namespace Common
{
    public interface IVerifier
    {
        bool Verify(List<double> input, List<double> template);
    }
}