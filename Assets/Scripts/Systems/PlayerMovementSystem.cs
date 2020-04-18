using Unity.Entities;
using Unity.Transforms;

public class PlayerMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref MovementData moveData, ref Translation translation) =>
        {
            moveData.direction.y += moveData.gravityRate * deltaTime;
            translation.Value += moveData.direction * (moveData.speed * deltaTime);
        }).ScheduleParallel();
    }
}