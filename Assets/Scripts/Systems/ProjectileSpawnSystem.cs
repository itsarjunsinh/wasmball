using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ProjectileSpawnSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<StateData>();
        RequireSingletonForUpdate<ProjectilePrefab>();
    }

    protected override void OnUpdate()
    {
        if (GetSingleton<StateData>().state == StateData.State.Playing)
        {
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((ref TurretData turretData, in Translation translation) =>
            {
                turretData.timer -= deltaTime;
                if (turretData.timer <= 0f)
                {
                    turretData.timer = 2f;
                    float spawnPositionX = translation.Value.x;
                    float spawnPositionY = translation.Value.y;

                    Entity player = GetSingletonEntity<PlayerTag>();
                    Translation target = EntityManager.GetComponentData<Translation>(player);

                    Entity pfProjectile = GetSingleton<ProjectilePrefab>().Value;
                    Entity projectile = EntityManager.Instantiate(pfProjectile);

                    EntityManager.AddComponentData(projectile, new Translation
                    {
                        Value = new float3(spawnPositionX, spawnPositionY, 0)
                    });
                    EntityManager.AddComponentData(projectile, new MovementData
                    {
                        direction = new float3(target.Value.x, target.Value.y, 0),
                        isAffectedByGravity = false,
                        gravityRate = 1,
                        speed = 3
                    });
                    EntityManager.AddComponentData(projectile, new ProjectileTag { });
                }
            }).WithStructuralChanges().Run();
        }
    }
}