using Unity.Entities;
using Unity.Tiny.Input;
using Unity.Transforms;

public class PlayerMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        float gravity = -2f;

        Entities.ForEach((ref MovementData moveData, ref Translation translation) =>
        {
            moveData.direction.y += gravity * deltaTime;
            translation.Value += moveData.direction * deltaTime;
        }).ScheduleParallel();
    }
}