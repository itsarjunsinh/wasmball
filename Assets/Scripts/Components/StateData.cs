using Unity.Entities;

[GenerateAuthoringComponent]
public struct StateData : IComponentData
{
    public enum State
    {
        Playing,
        Dead
    }

    public State state;
}