using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Inference;
using Unity.MLAgents.Policies;
using UnityEngine;

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
    public interface IActuator : IActionReceiver, IDiscreteActionMaskProvider
    {
        ActionSegment<int> DiscreteActions
        {
            get;
        }

        ActionSegment<float> ContinuousActions
        {
            get;
        }

        int TotalNumberOfActions
        {
            get;
        }

        ActuatorSpace DiscreteActuatorSpace
        {
            get;
        }

        ActuatorSpace ContinuousActuatorSpace
        {
            get;
        }

        /// <summary>
        /// Gets the name of this IActuator which will be used to sort it.
        /// </summary>
        /// <returns></returns>
        string GetName();

        void ResetData();
    }
}
