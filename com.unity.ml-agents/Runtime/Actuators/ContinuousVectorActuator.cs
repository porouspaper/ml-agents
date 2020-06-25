using System;
using Unity.MLAgents.Policies;

namespace Unity.MLAgents.Actuators
{
    public class ContinuousVectorActuator : BaseActuator
    {


        public ContinuousVectorActuator(int[] vectorActionSize) : base("ContinuousVectorActuator",
            vectorActionSize[0])
        {
            m_SpaceTypes = new SpaceType[vectorActionSize[0]];
            // Fill with Discrete SpaceType.
            for (int i = 0; i < m_SpaceTypes.Length; i++)
            {
                m_SpaceTypes[i] = SpaceType.Continuous;
            }
        }

        public override int[] GetBranchSizes()
        {
            return null;
        }

        public override void OnActionReceived()
        {
            // TODO: Log warning
        }

        public override void CollectDiscreteActionMasks(DiscreteActionMasker actionMasker)
        {
            // TODO: Log warning
        }
    }
}
