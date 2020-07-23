using System.Collections.Generic;

namespace Unity.MLAgents.Actuators
{
    /// <summary>
    /// Class that allows mutliple objects to write to its action mask buffer by using offsets.
    /// </summary>
    public class CompositeDiscreteActionMask : IDiscreteActionMask
    {
        public void SetMask(int branch, IEnumerable<int> actionIndices)
        {
            throw new System.NotImplementedException();
        }

        public bool[] GetMask()
        {
            throw new System.NotImplementedException();
        }

        public void ResetMask()
        {
            throw new System.NotImplementedException();
        }
    }
}
