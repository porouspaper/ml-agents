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
        ActuatorSpace m_ActuatorSpace;

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
                    m_ActuatorSpace = ActuatorSpace.MakeContinuous(vectorActionSize[0]);
                    suffix = "-Continuous";
                    break;
                case SpaceType.Discrete:
                    m_ActuatorSpace = ActuatorSpace.MakeDiscrete(vectorActionSize);
                    suffix = "-Discrete";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(spaceType),
                        spaceType,
                        "Unknown enum value.");
            }
            m_Name = name + suffix;
        }

        public ActionSegment Actions { get; private set; }

        public ActuatorSpace GetActuatorSpace()
        {
            return m_ActuatorSpace;
        }

        public void ResetData()
        {
            Actions = new ActionSegment();
        }

        public void OnActionReceived(ActionSegment actions)
        {
            Actions = actions;
            m_ActionReceiver.OnActionReceived(Actions);
        }

        public void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
        {
            if (m_SpaceType == SpaceType.Discrete)
            {
                // TODO: Call into agent?
            }
        }

        public string GetName()
        {
            return m_Name;
        }
    }
}
