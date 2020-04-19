using Unity.Entities;
using Unity.Tiny.Input;

public class PlayerInputSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<StateData>();
    }
    protected override void OnUpdate()
    {
        if (GetSingleton<StateData>().state == StateData.State.Playing)
        {
            var input = World.GetOrCreateSystem<InputSystem>();
            bool isTapped = input.GetMouseButtonDown(0);

            Entities.WithAll<PlayerTag>().ForEach((ref MovementData moveData) =>
            {
                if (isTapped)
                {
                    moveData.direction.y += 8f; // Amount to add to the direction
                }
            }).ScheduleParallel();
        }
    }
}