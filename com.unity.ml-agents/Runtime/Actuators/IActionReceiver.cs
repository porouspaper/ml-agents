using System;
using System.Collections;
using System.Collections.Generic;

namespace Unity.MLAgents.Actuators
{
    public readonly struct ActionSegment : IEnumerable
    {
        readonly float[] m_ActionArray;
        public readonly int Offset;
        public readonly int Length;
        public ActionSegment(float[] actionArray, int offset, int length)
        {
            m_ActionArray = actionArray;
            Offset = offset;
            Length = length;
        }

        public float[] Array
        {
            get { return m_ActionArray; }
        }

        public float this[int index]
        {
            get
            {
                if (index < 0 || index > Length)
                {
                    throw new IndexOutOfRangeException($"Index out of bounds, expected a number between 0 and {Length}");
                }
                return m_ActionArray[Offset + index];
            }
        }
        public IEnumerator GetEnumerator()
        {
            return m_ActionArray.GetEnumerator();
        }

        public float[] ToArray()
        {
            var actions = new float[Length];
            System.Array.Copy(m_ActionArray, Offset, actions, 0, Length);
            return actions;
        }
    }
    public interface IActionReceiver
    {
        /// <summary>
        ///  This method is called in order to allow the user execution actions
        /// with the array of actions passed in.
        /// </summary>
        /// <param name="actions">The list of actions to perform.</param>
        void OnActionReceived(ActionSegment actions);
    }
}
