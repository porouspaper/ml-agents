using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Unity.MLAgents.Actuators
{
    public class ActuatorList : IList<IActuator>
    {
        float[] m_Actions;
        IList<IActuator> m_Actuators;

        public ActuatorList(int capacity = 0)
        {
            m_Actuators = new List<IActuator>(capacity);
        }

        public float[] StoredActions
        {
            get { return m_Actions; }
        }

        public void EnsureActionBufferSize()
        {
            var size = 0;
            for (int i = 0; i < m_Actuators.Count; i++)
            {
                size += m_Actuators[i].GetNumberOfActions();
            }

            m_Actions = new float[size];
        }

        public void UpdateActions(float[] fullActionBuffer)
        {
            if (fullActionBuffer == null)
            {
                Array.Clear(m_Actions, 0, m_Actions.Length);
            }
            else
            {
                Array.Copy(fullActionBuffer, m_Actions, m_Actions.Length);
            }
            var start = 0;
            for (int i = 0; i < m_Actuators.Count; i++)
            {
                var actuator = m_Actuators[i];
                actuator.UpdateActions(m_Actions, start);
                start += actuator.GetNumberOfActions();
            }
        }

        public void ExecuteActions()
        {
            for (int i = 0; i < m_Actuators.Count; i++)
            {
                m_Actuators[i].OnActionReceived();
            }
        }

        public void SortActuators()
        {
            ((List<IActuator>)m_Actuators).Sort((x, y) => x.GetName().CompareTo(y.GetName()));
        }

        public IEnumerator<IActuator> GetEnumerator()
        {
            return m_Actuators.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_Actuators).GetEnumerator();
        }

        public void Add(IActuator item)
        {
            m_Actuators.Add(item);
        }

        public void Clear()
        {
            m_Actuators.Clear();
        }

        public bool Contains(IActuator item)
        {
            return m_Actuators.Contains(item);
        }

        public void CopyTo(IActuator[] array, int arrayIndex)
        {
            m_Actuators.CopyTo(array, arrayIndex);
        }

        public bool Remove(IActuator item)
        {
            return m_Actuators.Remove(item);
        }

        public int Count => m_Actuators.Count;

        public bool IsReadOnly => m_Actuators.IsReadOnly;

        public int IndexOf(IActuator item)
        {
            return m_Actuators.IndexOf(item);
        }

        public void Insert(int index, IActuator item)
        {
            m_Actuators.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            m_Actuators.RemoveAt(index);
        }

        public IActuator this[int index]
        {
            get => m_Actuators[index];
            set => m_Actuators[index] = value;
        }

    }
}
