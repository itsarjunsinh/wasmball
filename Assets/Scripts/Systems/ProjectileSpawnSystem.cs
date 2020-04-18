using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

class ProjectileSpawnSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<ProjectilePrefab>();
    }

    protected override void OnUpdate()
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

                Entity pfProjectile = GetSingleton<ProjectilePrefab>().Value;
                Entity projectile = EntityManager.Instantiate(pfProjectile);
                EntityManager.SetComponentData(projectile, new Translation
                {
                    Value = new float3(spawnPositionX, spawnPositionY, 0)
                });
                EntityManager.SetComponentData(projectile, new MovementData
                {
                    direction = new float3(0, 0, 0),
                    isAffectedByGravity = false,
                    gravityRate = 1,
                    speed = 2
                });
            }
        }).WithStructuralChanges().Run();
    }
}