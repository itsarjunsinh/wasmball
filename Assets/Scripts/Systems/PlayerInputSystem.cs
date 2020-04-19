using Unity.Entities;
using Unity.Tiny.Input;
using Unity.Transforms;

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

            Entities.WithAll<PlayerTag>().ForEach((ref MovementData moveData, in Translation translation) =>
            {
                if (isTapped)
                {
                    if (moveData.direction.y + 8f <=31)
                    {
                        moveData.direction.y += 8f; // Amount to add to the direction
                    }
                    else
                    {
                        moveData.direction.y = 31; // Hardcoded value of Max height
                    }
                }
            }).ScheduleParallel();
        }
    }
}