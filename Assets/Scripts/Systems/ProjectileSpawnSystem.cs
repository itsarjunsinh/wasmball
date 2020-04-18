using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

class ProjectileSpawnSystem : SystemBase
{
    bool spawned = false;
    EntityManager entityManager;

    protected override void OnCreate()
    {
        RequireSingletonForUpdate<ProjectilePrefab>();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        if (!spawned)
        {
            var pfHolderEntity = GetSingletonEntity<ProjectilePrefab>();
            var pfProjectile = GetComponent<ProjectilePrefab>(pfHolderEntity);
            var projectile = entityManager.Instantiate(pfProjectile.Value);
            entityManager.SetComponentData(projectile, new Translation
            {
                Value = new float3(0, 0, 0)
            });
            spawned = true;
        }
    }
}