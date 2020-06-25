using System.Collections.Generic;
using Unity.MLAgents.Policies;

namespace Unity.MLAgents.Actuators
{
    /// <summary>
    /// Abstraction that facilitates the execution of actions.
    /// </summary>
    public interface IActuator
    {
        float[] Actions
        {
            get;
        }

        void UpdateActions(float[] fullActionBuffer, int offset);

        /// <summary>
        /// Get the number of actions this IActuator will execute on.
        /// </summary>
        int GetNumberOfActions();

        /// <summary>
        /// Get the sizes of each discrete branch if they exist.  The length of this
        /// array is equal to what is returned from <see cref="GetNumberOfActions"/>.
        /// </summary>
        /// <returns></returns>
        int[] GetBranchSizes();

        void ResetData();

        /// <summary>
        ///  This method is called in order to allow the user execution actions
        /// with the array of actions passed in.
        /// </summary>
        void OnActionReceived();

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
