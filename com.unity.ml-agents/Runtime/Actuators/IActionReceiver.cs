using System;
using System.Collections;
using System.Collections.Generic;

namespace Unity.MLAgents.Actuators
{
    public readonly struct ActionSegment<T> : IEnumerable<T>
    where T : struct
    {
        readonly T[] m_ActionArray;
        public readonly int Offset;
        public readonly int Length;

        public static ActionSegment<T> Empty = new ActionSegment<T>(System.Array.Empty<T>(), 0, 0);

        public static ActionSegment<T> MakeActionSegment(T[] actionArray, int offset, int length)
        {
            if (length == 0)
            {
                return Empty;
            }
            return new ActionSegment<T>(actionArray, offset, length);
        }
        public ActionSegment(T[] actionArray, int offset, int length)
        {
            m_ActionArray = actionArray;
            Offset = offset;
            Length = length;
        }

        public T[] Array
        {
            get { return m_ActionArray; }
        }

        public T this[int index]
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

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)m_ActionArray).GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return m_ActionArray.GetEnumerator();
        }
    }

    public interface IActionReceiver
    {
        /// <summary>
        ///  This method is called in order to allow the user execution actions
        /// with the array of actions passed in.
        /// </summary>
        /// <param name="continuousActions">The list of continuous actions to perform.</param>
        /// <param name="discreteActions">The list of discrete actions to perform.</param>
        void OnActionReceived(ActionSegment<float> continuousActions, ActionSegment<int> discreteActions);
    }
}
