using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class AABBSystem : JobComponentSystem
{
    [BurstCompile]
    struct AABBSystemJob : IJobForEach<Translation, AABB>
    {
        public void Execute(
            [ReadOnly] ref Translation translation,
            ref AABB aabb
            )
        {
            aabb.Value.c0 = translation.Value + aabb.Position;
            aabb.Value.c1 = aabb.Value.c0 + aabb.Size;

        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new AABBSystemJob();
        
        return job.Schedule(this, inputDeps);
    }
}
