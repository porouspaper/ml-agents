using System;

namespace Unity.MLAgents.Actuators
{
    public interface IActionReceiver
    {
        /// <summary>
        ///  This method is called in order to allow the user execution actions
        /// with the array of actions passed in.
        /// </summary>
        /// <param name="actions">The list of actions to perform</param>
        void OnActionReceived(ArraySegment<float> actions);
    }
}
