using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// interface for the functions used for verification/identification
    /// </summary>
    public interface IVerifier
    {
        bool Verify(List<double> input, List<double> template);
    }
}