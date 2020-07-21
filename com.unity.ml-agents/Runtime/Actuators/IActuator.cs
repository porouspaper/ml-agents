using System;
using Unity.MLAgents.Policies;

namespace Unity.MLAgents.Actuators
{
    public struct ActuatorSpace
    {
        public readonly SpaceType[] SpaceTypes;
        public readonly int[] BranchSizes;
        public readonly int NumActions;

        public static ActuatorSpace MakeContinuous(int numActions)
        {
            var spaceTypes = new SpaceType[numActions];
            for (var i = 0; i < numActions; i++)
            {
                spaceTypes[i] = SpaceType.Continuous;
            }
            var actuatorSpace = new ActuatorSpace(spaceTypes, numActions);
            return actuatorSpace;
        }

        public static ActuatorSpace MakeDiscrete(int[] branchSizes)
        {
            var numActions = branchSizes.Length;
            var spaceTypes = new SpaceType[numActions];
            for (var i = 0; i < numActions; i++)
            {
                spaceTypes[i] = SpaceType.Discrete;
            }
            var actuatorSpace = new ActuatorSpace(spaceTypes, numActions, branchSizes);
            return actuatorSpace;
        }

        ActuatorSpace(SpaceType[] spaceTypes, int numActions, int[] branchSizes = null)
        {
            SpaceTypes = spaceTypes;
            NumActions = numActions;
            BranchSizes = branchSizes;
        }
    }
    /// <summary>
    /// Abstraction that facilitates the execution of actions.
    /// </summary>
    public interface IActuator : IActionReceiver
    {
        ArraySegment<float> Actions
        {
            get;
        }

        ActuatorSpace GetActuatorSpace();

        /// <summary>
        /// Collects masks for discrete actions, please refer to
        /// <see cref="Agent.CollectDiscreteActionMasks"/>.
        /// </summary>
        /// <param name="actionMasker"></param>
        void CollectDiscreteActionMasks(DiscreteActionMasker actionMasker);

        /// <summary>
        /// Gets the name of this IActuator which will be used to sort it.
        /// </summary>
        /// <returns></returns>
        string GetName();
    }
}
