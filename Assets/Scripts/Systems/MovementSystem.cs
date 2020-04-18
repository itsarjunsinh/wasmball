using Unity.Entities;
using Unity.Transforms;

public class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref Translation translation, ref MovementData moveData) =>
        {
            if (moveData.isAffectedByGravity)
            {
                moveData.direction.y += moveData.gravityRate * deltaTime;
            }
            translation.Value += (moveData.direction - translation.Value) * (moveData.speed * deltaTime);
        }).ScheduleParallel();
    }
}