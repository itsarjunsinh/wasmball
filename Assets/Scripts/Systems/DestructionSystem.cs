using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

class DestructionSystem : SystemBase
{
    bool selfDestructCompleted = false;
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<StateData>();
        RequireSingletonForUpdate<PlayerTag>();
    }
    protected override void OnUpdate()
    {
        StateData stateData = GetSingleton<StateData>();
        if (stateData.state == StateData.State.WaitingToPlay)
        {
            if (!selfDestructCompleted)
            {
                // Destroy all projectiles
                EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
                Entities.WithAll<ProjectileTag>().ForEach((ref Entity entity) =>
                {
                    EntityManager.DestroyEntity(entity);
                }).WithStructuralChanges().Run();
                ecb.Playback(EntityManager);
                ecb.Dispose();
                selfDestructCompleted = true;
            }
        }
        if (stateData.state == StateData.State.Playing)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            Entities.WithAll<ProjectileTag>().ForEach((ref Entity entity, ref MovementData data, ref Translation translation) =>
            {
                if ((Math.Abs(data.direction.x - translation.Value.x)) <= 0.5)
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
                        selfDestructCompleted = false;
                        stateData.state = StateData.State.Dead;
                        SetSingleton(stateData);
                        //ecb.DestroyEntity(player); - Don't destroy player entity.
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