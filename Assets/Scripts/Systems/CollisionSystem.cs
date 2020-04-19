using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

class CollisionSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<StateData>();
        RequireSingletonForUpdate<PlayerTag>();
    }
    protected override void OnUpdate()
    {
        if (GetSingleton<StateData>().state == StateData.State.Playing)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            Entities.WithAll<ProjectileTag>().ForEach((ref Entity entity, ref MovementData data, ref Translation translation) =>
            {
                if ((Mathf.Abs(data.direction.x - translation.Value.x)) <= 0.5)
                {
                    // TODO : Move player destruction logic to another system.
                    Entity player = GetSingletonEntity<PlayerTag>();
                    Translation playerPosition = EntityManager.GetComponentData<Translation>(player);
                    float playerHeight = EntityManager.GetComponentData<PlayerHeight>(player).Value;

                    float projectileY = translation.Value.y;
                    float playerYBound, distanceToPlayer;
                    if (projectileY > playerPosition.Value.y)
                    {
                        playerYBound = playerPosition.Value.y + (playerHeight / 2);
                        distanceToPlayer = projectileY - playerYBound;

                    }
                    else
                    {
                        playerYBound = playerPosition.Value.y - (playerHeight / 2);
                        distanceToPlayer = playerYBound - projectileY;
                    }

                    if (distanceToPlayer <= 0.5)
                    {
                        StateData stateData = GetSingleton<StateData>();
                        stateData.state = StateData.State.Dead;
                        SetSingleton(stateData);
                        ecb.DestroyEntity(player);
                    }
                    else
                    {
                        ecb.DestroyEntity(entity);
                    }
                }
            }).WithoutBurst().Run();
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}