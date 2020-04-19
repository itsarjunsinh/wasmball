using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

class CollisionSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<StateData>();
        var projectileQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerTag>());
        RequireForUpdate(projectileQuery);
    }
    protected override void OnUpdate()
    {
        if (GetSingleton<StateData>().state == StateData.State.Playing)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            Entities.WithAll<ProjectileTag>().ForEach((ref MovementData data, ref Translation translation) =>
            {
                if ((Mathf.Abs(data.direction.x - translation.Value.x)) < 0.5)
                {
                    // TODO : Check if collided
                }
            }).WithoutBurst().Run();
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}