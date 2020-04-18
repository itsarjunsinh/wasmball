using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

class ProjectileSpawnSystem : SystemBase
{
    EntityManager entityManager;

    protected override void OnCreate()
    {
        RequireSingletonForUpdate<ProjectilePrefab>();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
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
                var pfHolderEntity = GetSingletonEntity<ProjectilePrefab>();
                var pfProjectile = GetComponent<ProjectilePrefab>(pfHolderEntity);
                var projectile = entityManager.Instantiate(pfProjectile.Value);

                var x = translation.Value.x;
                var y = translation.Value.y;
                entityManager.SetComponentData(projectile, new Translation
                {
                    Value = new float3(x, y, 0)
                });
            }
        }).WithStructuralChanges().WithoutBurst().Run();
    }
}