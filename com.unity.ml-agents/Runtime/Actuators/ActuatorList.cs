using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.MLAgents.Actuators
{
    /// <summary>
    /// A list of IActuators
    /// </summary>
    public class ActuatorList : IList<IActuator>
    {
        float[] m_Actions;
        IList<IActuator> m_Actuators;

        /// <summary>
        /// Create an ActuatorList with a preset capacity.
        /// </summary>
        /// <param name="capacity">The capacity of the list to create.</param>
        public ActuatorList(int capacity = 0)
        {
            m_Actuators = new List<IActuator>(capacity);
        }

        /// <summary>
        /// Returns the previously stored actions for the actuators in this list.
        /// </summary>
        public float[] StoredActions
        {
            get { return m_Actions; }
        }

        /// <summary>
        /// Ensures that the action buffer size is correct based on the number of
        /// actions each actuator has.
        /// </summary>
        public void EnsureActionBufferSize()
        {
            var size = 0;
            for (var i = 0; i < m_Actuators.Count; i++)
            {
                size += m_Actuators[i].GetActuatorSpace().NumActions;
            }

            m_Actions = new float[size];
        }

        /// <summary>
        /// Updates the local action buffer with the action buffer passed in.  If the buffer
        /// passed in is null, the local action buffer will be cleared.
        /// </summary>
        /// <param name="fullActionBuffer">The action buffer which contains all of the
        /// actions for the IActuators in this list.</param>
        public void UpdateActions(float[] fullActionBuffer)
        {
            if (fullActionBuffer == null)
            {
                Array.Clear(m_Actions, 0, m_Actions.Length);
            }
            else
            {
                Debug.Assert(fullActionBuffer.Length == m_Actions.Length,
                    "fullActionBuffer is a different size than m_Actions.");

                Array.Copy(fullActionBuffer, m_Actions, m_Actions.Length);
            }
        }

        /// <summary>
        /// Iterates through all of the IActuators in this list and calls their
        /// <see cref="IActionReceiver.OnActionReceived"/> method on them.
        /// </summary>
        public void ExecuteActions()
        {
            var start = 0;
            for (var i = 0; i < m_Actuators.Count; i++)
            {
                var actuator = m_Actuators[i];
                var numActions = actuator.GetActuatorSpace().NumActions;
                actuator.OnActionReceived(new ArraySegment<float>(m_Actions, start, numActions));
                start += numActions;
            }
        }

        /// <summary>
        /// Sorts the <see cref="IActuator"/>s according to their <see cref="IActuator.GetName"/> value.
        /// </summary>
        public void SortActuators()
        {
            ((List<IActuator>) m_Actuators).Sort((x,
                y) => x.GetName()
                .CompareTo(y.GetName()));
        }

        /// <summary>
        /// Resets the data of the local action buffer to all 0f.
        /// </summary>
        public void ResetData()
        {
            Array.Clear(m_Actions, 0, m_Actions.Length);
        }

        /*********************************************************************************
         * IList implementation that delegates to m_Actuators List.                      *
         *********************************************************************************/

        /// <summary>
        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        /// </summary>
        public IEnumerator<IActuator> GetEnumerator()
        {
            return m_Actuators.GetEnumerator();
        }

        /// <summary>
        /// <inheritdoc cref="IList{T}.GetEnumerator"/>
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_Actuators).GetEnumerator();
        }

        /// <summary>
        /// <inheritdoc cref="ICollection{T}.Add"/>
        /// </summary>
        /// <param name="item"></param>
        public void Add(IActuator item)
        {
            m_Actuators.Add(item);
        }

        /// <summary>
        /// <inheritdoc cref="ICollection{T}.Clear"/>
        /// </summary>
        public void Clear()
        {
            m_Actuators.Clear();
        }

        /// <summary>
        /// <inheritdoc cref="ICollection{T}.Contains"/>
        /// </summary>
        public bool Contains(IActuator item)
        {
            return m_Actuators.Contains(item);
        }

        /// <summary>
        /// <inheritdoc cref="ICollection{T}.CopyTo"/>
        /// </summary>
        public void CopyTo(IActuator[] array, int arrayIndex)
        {
            m_Actuators.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// <inheritdoc cref="ICollection{T}.Remove"/>
        /// </summary>
        public bool Remove(IActuator item)
        {
            return m_Actuators.Remove(item);
        }

        /// <summary>
        /// <inheritdoc cref="ICollection{T}.Count"/>
        /// </summary>
        public int Count => m_Actuators.Count;

        /// <summary>
        /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
        /// </summary>
        public bool IsReadOnly => m_Actuators.IsReadOnly;

        /// <summary>
        /// <inheritdoc cref="IList{T}.IndexOf"/>
        /// </summary>
        public int IndexOf(IActuator item)
        {
            return m_Actuators.IndexOf(item);
        }

        /// <summary>
        /// <inheritdoc cref="IList{T}.Insert"/>
        /// </summary>
        public void Insert(int index, IActuator item)
        {
            m_Actuators.Insert(index, item);
        }

        /// <summary>
        /// <inheritdoc cref="IList{T}.RemoveAt"/>
        /// </summary>
        public void RemoveAt(int index)
        {
            m_Actuators.RemoveAt(index);
        }

        /// <summary>
        /// <inheritdoc cref="IList{T}.this"/>
        /// </summary>
        public IActuator this[int index]
        {
            get => m_Actuators[index];
            set => m_Actuators[index] = value;
        }

    }
}
