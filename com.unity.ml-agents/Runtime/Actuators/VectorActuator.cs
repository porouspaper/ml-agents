using System;
using Unity.MLAgents.Policies;

namespace Unity.MLAgents.Actuators
{
    class VectorActuator : IActuator
    {
        // Easy access for now about which space type to use for business logic.
        // Should be removed once a mixed SpaceType NN is available.
        SpaceType m_SpaceType;
        string m_Name;
        IActionReceiver m_ActionReceiver;

        public VectorActuator(IActionReceiver actionReceiver,
            int[] vectorActionSize,
            SpaceType spaceType,
            string name = "VectorActuator")
        {
            m_ActionReceiver = actionReceiver;
            m_SpaceType = spaceType;
            string suffix;
            switch (m_SpaceType)
            {
                case SpaceType.Continuous:
                    ContinuousActuatorSpace = ActuatorSpace.MakeContinuous(vectorActionSize[0]);
                    DiscreteActuatorSpace = ActuatorSpace.MakeDiscrete(Array.Empty<int>());
                    suffix = "-Continuous";
                    break;
                case SpaceType.Discrete:
                    DiscreteActuatorSpace = ActuatorSpace.MakeDiscrete(vectorActionSize);
                    ContinuousActuatorSpace = ActuatorSpace.MakeContinuous(0);
                    suffix = "-Discrete";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(spaceType),
                        spaceType,
                        "Unknown enum value.");
            }
            m_Name = name + suffix;
        }

        public void ResetData()
        {
            DiscreteActions = new ActionSegment<int>();
            ContinuousActions = new ActionSegment<float>();
        }

        public void OnActionReceived(ActionSegment<float> continuousActions, ActionSegment<int> discreteActions)
        {
            ContinuousActions = continuousActions;
            DiscreteActions = discreteActions;
            m_ActionReceiver.OnActionReceived(ContinuousActions, DiscreteActions);
        }

        public void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
        {
            if (m_SpaceType == SpaceType.Discrete)
            {
                // TODO: Call into agent?
            }
        }

        public ActionSegment<int> DiscreteActions { get; private set; }
        public ActionSegment<float> ContinuousActions { get; private set; }
        public int TotalNumberOfActions
        {
            get { return ContinuousActuatorSpace.NumActions + DiscreteActuatorSpace.NumActions; }
        }
        public ActuatorSpace DiscreteActuatorSpace { get; }
        public ActuatorSpace ContinuousActuatorSpace { get; }

        public string GetName()
        {
            return m_Name;
        }
    }
}
