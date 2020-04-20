using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class StateSystem : SystemBase
{
    bool initalValuesSet = false;
    bool uiHidden = false;

    protected override void OnCreate()
    {
        RequireSingletonForUpdate<StateData>();
        RequireSingletonForUpdate<PlayerTag>();
        RequireSingletonForUpdate<ProjectilePrefab>();
    }
    protected override void OnUpdate()
    {
        StateData stateData = GetSingleton<StateData>();
        if (stateData.state == StateData.State.WaitingToPlay)
        {
            if (!initalValuesSet)
            {
                // Set inital values
                uiHidden = false;
                initalValuesSet = true;
            }
        }
        if (stateData.state == StateData.State.Playing)
        {
            if (!uiHidden)
            {
                // TODO: Hide UI
                uiHidden = true;
            }
        }
        if (stateData.state == StateData.State.Dead)
        {
            // TODO: Reset all systems and components
            Entities.WithAll<PlayerTag>().ForEach((ref MovementData moveData, ref Translation translation) =>
            {
                float3 defaultLocation = new float3(0, 29, 0);
                moveData.direction = defaultLocation;
                translation.Value = defaultLocation;
            }).ScheduleParallel();
            stateData.state = StateData.State.WaitingToPlay;
            SetSingleton(stateData);
            initalValuesSet = false;
        }
    }
}