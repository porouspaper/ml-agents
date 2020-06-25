using System;
using System.Collections.Generic;
using Unity.MLAgents.Policies;

namespace Unity.MLAgents.Actuators
{
    public abstract class BaseActuator : IActuator
    {
        protected SpaceType[] m_SpaceTypes;
        string m_Name;
        int m_NumActions;
        ArraySegment<float> m_ActionsView;

        public BaseActuator(string name, int numActions)
        {
            m_Name = name;
            m_NumActions = numActions;
            Actions = new float[numActions];
        }

        public float[] Actions { get; set; }

        public int GetNumberOfActions()
        {
            return m_NumActions;
        }

        public abstract int[] GetBranchSizes();

        public void UpdateActions(float[] fullActionBuffer, int offset)
        {
            Array.Copy(fullActionBuffer, offset, Actions, 0, m_NumActions);
        }

        public void ResetData()
        {
            for (var i = 0; i < m_NumActions; i++)
            {
                Actions[i] = 0;
            }
        }

        public abstract void OnActionReceived();
        public abstract void CollectDiscreteActionMasks(DiscreteActionMasker actionMasker);

        public string GetName()
        {
            return m_Name;
        }
    }
}
