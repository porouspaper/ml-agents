using System;
using Unity.MLAgents.Policies;

namespace Unity.MLAgents.Actuators
{
    public class DiscreteVectorActuator : BaseActuator
    {
        readonly int[] m_VectorActionSize;
        public DiscreteVectorActuator(int[] vectorActionSize)
            : base("DiscreteVectorActuator", vectorActionSize.Length)
        {
            m_VectorActionSize = vectorActionSize;
            m_SpaceTypes = new SpaceType[vectorActionSize.Length];
            // Fill with Discrete SpaceType.
            for (int i = 0; i < m_SpaceTypes.Length; i++)
            {
                m_SpaceTypes[i] = SpaceType.Discrete;
            }
        }

        public override int[] GetBranchSizes()
        {
            return m_VectorActionSize;
        }

        public override void OnActionReceived()
        {

        }

        public override void CollectDiscreteActionMasks(DiscreteActionMasker actionMasker)
        {
            // TODO: Log warning
        }
    }
}
