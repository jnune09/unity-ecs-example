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
            // reset collision value
            collision.Value = float3.zero;

            for (int j = index + 1; j < colliders.Length; j++)
            {
                if (SimplePhysics.Intersection(colliders[index].Value, colliders[j].Value))
                {
                    bool4 detection = SimplePhysics.Collision(colliders[index].Value, colliders[j].Value);
                    if (detection.x)
                    {
                        collision.Value.y = -1f;
                    }
                    if (detection.y)
                    {
                        collision.Value.y = 1f;
                    }
                    if (detection.z)
                    {
                        collision.Value.x = 1f;
                    }
                    if (detection.w)
                    {
                        collision.Value.x = -1f;
                    }
                }
            }

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
