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
                turretData.timer = 3f;
                
                float spawnPositionX = translation.Value.x;
                float spawnPositionY = translation.Value.y;

                Entity pfProjectile = GetSingleton<ProjectilePrefab>().Value;
                Entity projectile = EntityManager.Instantiate(pfProjectile);
                EntityManager.SetComponentData(projectile, new Translation
                {
                    Value = new float3(spawnPositionX, spawnPositionY, 0)
                });
            }
        }).WithStructuralChanges().Run();
    }
}