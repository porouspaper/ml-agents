namespace Unity.MLAgents.Actuators
{
    public interface IDiscreteActionMaskProvider
    {
        void WriteDiscreteActionMask(IDiscreteActionMask actionMask);
    }
}
