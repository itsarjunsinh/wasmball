using Unity.Entities;
using Unity.Transforms;

public class MovementSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<StateData>();
    }

    protected override void OnUpdate()
    {
        if (GetSingleton<StateData>().state == StateData.State.Playing)
        {
            float deltaTime = Time.DeltaTime;
            Entities.ForEach((ref MovementData moveData, ref Translation translation) =>
            {
                if (moveData.isAffectedByGravity)
                {
                    if (translation.Value.y > 7) // Hardcoded value of Min height
                    {
                        moveData.direction.y += moveData.gravityRate * deltaTime;
                    }
                }
                translation.Value += (moveData.direction - translation.Value) * (moveData.speed * deltaTime);
            }).ScheduleParallel();
        }
    }
}