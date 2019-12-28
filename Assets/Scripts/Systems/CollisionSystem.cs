using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class CollisionSystem : JobComponentSystem
{
    [BurstCompile]
    struct CollisionSystemJob : IJobForEachWithEntity<AABB, Collision>
    {
        [ReadOnly] public NativeArray<AABB> colliders;

        public void Execute(Entity entity, int index, 
            [ReadOnly] ref AABB aabb,
            ref Collision collision
            )
        {
            float3 value = new float3();

            for (int j = index + 1; j < colliders.Length; j++)
            {
                if (SimplePhysics.Intersection(colliders[index].Value, colliders[j].Value))
                {
                    bool4 collisionDetection = SimplePhysics.Collision(colliders[index].Value, colliders[j].Value);
                    if (collisionDetection.x)
                    {
                        value.y = -1f;
                    }
                    if (collisionDetection.y)
                    {
                        value.y = 1f;
                    }
                    if (collisionDetection.z)
                    {
                        value.x = 1f;
                    }
                    if (collisionDetection.w)
                    {
                        value.x = -1f;
                    }
                }
            }

            collision.Value = value;
        }
    }

    EntityQuery m_aabbQuery;

    protected override void OnCreate()
    {
        m_aabbQuery = GetEntityQuery(typeof(AABB));
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var colliders = m_aabbQuery.ToComponentDataArray<AABB>(Allocator.TempJob);

        var job = new CollisionSystemJob
        {
            colliders = colliders
        };
        
        var jobHandle = job.Schedule(this, inputDeps);

        jobHandle.Complete();

        colliders.Dispose();

        return jobHandle;
    }
}
