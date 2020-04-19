using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

// TODO: Stop multiple projectiles from spawning at the same location
public class ProjectileSpawnSystem : SystemBase
{
    int maxProjectileSpawn = 3;
    float spawnTimer = 2; // Initial wait time
    float2[] spawnPoints = new float2[]
    {
        new float2(30, 30), // Top Right
        new float2(30, 20), // Center Right
        new float2(30, 5), // Bottom Right
        new float2(-30, 5), // Bottom Left
        new float2(-30, 20), // Center Left
        new float2(-30, 30) // Top Left
    };

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
            spawnTimer -= deltaTime;

            if (spawnTimer <= 0f)
            {
                spawnTimer = 1f;
                EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
                Entity pfProjectile = GetSingleton<ProjectilePrefab>().Value;
                Entity player = GetSingletonEntity<PlayerTag>();

                for (int i = 0; i < maxProjectileSpawn; i++)
                {
                    int randomSpawnPoint = Random.Range(0, 6); // Hardcoded length of spawnpoints array
                    float2 start = spawnPoints[randomSpawnPoint];
                    float3 target = GetComponent<Translation>(player).Value;

                    var projectile = ecb.Instantiate(pfProjectile);
                    ecb.SetComponent(projectile, new ProjectileTag { });
                    ecb.SetComponent(projectile, new Translation
                    {
                        Value = new float3(start.x, start.y, 0)
                    });
                    ecb.SetComponent(projectile, new MovementData
                    {
                        direction = new float3(target.x, target.y, 0),
                        isAffectedByGravity = false,
                        gravityRate = 1,
                        speed = 3
                    });
                }
                ecb.Playback(EntityManager);
                ecb.Dispose();
            }
        }
    }
}