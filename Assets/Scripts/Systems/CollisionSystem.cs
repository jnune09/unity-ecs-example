using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class CollisionSystem : JobComponentSystem
{
    [BurstCompile]
    struct CollisionSystemJob : IJobForEachWithEntity<AABB>
    {
        [ReadOnly] public NativeArray<AABB> colliders;

        public void Execute(Entity entity, int index, ref AABB aabb)
        {
            for (int j = index + 1; j < colliders.Length; j++)
            {
                if (SimplePhysics.Intersect(colliders[index], colliders[j]))
                {
                    //UnityEngine.Debug.Log("collision detected!");
                }
            }
        }
    }

    EntityQuery m_aabbQuery;

    protected override void OnCreate()
    {
        var query = new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(AABB) }
        };
        m_aabbQuery = GetEntityQuery(query);
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
