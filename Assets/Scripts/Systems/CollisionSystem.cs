using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

class CollisionSystem : SystemBase
{
    protected override void OnCreate()
    {
        var projectileQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerTag>());
        RequireForUpdate(projectileQuery);
    }
    protected override void OnUpdate()
    {
        Entities.WithAll<ProjectileTag>().ForEach((ref MovementData data, ref Translation translation) =>
        {
            if((Mathf.Abs(data.direction.x - translation.Value.x)) < 0.5) {
                // TODO: Check if collided
            }
        }).ScheduleParallel();
    }
}