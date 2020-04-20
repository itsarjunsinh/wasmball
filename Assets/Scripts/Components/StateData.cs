using Unity.Entities;

[GenerateAuthoringComponent]
public struct StateData : IComponentData
{
    public enum State
    {
        WaitingToPlay,
        Playing,
        Dead
    }

    public State state;
}