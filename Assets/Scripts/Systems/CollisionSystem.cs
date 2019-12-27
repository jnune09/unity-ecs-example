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
            collision.Top = false;
            collision.Bottom = false;
            collision.Left = false;
            collision.Right = false;

            for (int j = index + 1; j < colliders.Length; j++)
            {
                if (SimplePhysics.Intersect(colliders[index], colliders[j]))
                {
                    bool4 collide = SimplePhysics.Collision(colliders[index], colliders[j]);
                    if (collide.x)
                    {
                        collision.Top = true;
                    }
                    if (collide.y)
                    {
                        collision.Bottom = true;
                    }
                    if (collide.z)
                    {
                        collision.Left = true;
                    }
                    if (collide.w)
                    {
                        collision.Right = true;
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
