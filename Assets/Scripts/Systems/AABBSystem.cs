using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class AABBSystem : JobComponentSystem
{
    [BurstCompile]
    struct AABBSystemJob : IJobForEach<AABB, Translation>
    {
        public void Execute(ref AABB aabb, [ReadOnly] ref Translation translation)
        {
            aabb.Min = translation.Value - 0.5f;
            aabb.Max = translation.Value + 0.5f;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new AABBSystemJob();
        

        return job.Schedule(this, inputDeps);
    }
}
