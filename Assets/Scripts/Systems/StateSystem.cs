using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class StateSystem : SystemBase
{
    bool isFirstRun = true;
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
            if (!initalValuesSet && !isFirstRun)
            {
                // Set inital values
                Entities.WithAll<UiTag>().ForEach((Entity entity) =>
                {
                    EntityManager.RemoveComponent<Disabled>(entity);
                }).WithStructuralChanges().Run();
                uiHidden = false;
                initalValuesSet = true;
            }
        }
        if (stateData.state == StateData.State.Playing)
        {
            if (!uiHidden)
            {
                // TODO: Hide UI
                Entity ui = GetSingletonEntity<UiTag>();
                EntityManager.AddComponent<Disabled>(ui);
                Entities.WithAll<UiTag>().ForEach((Entity entity) =>
                {
                    EntityManager.AddComponent<Disabled>(entity);
                }).WithStructuralChanges().Run();
                uiHidden = true;
                isFirstRun = false;
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