using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class InteractionSystem : JobComponentSystem
{
    [BurstCompile]
    struct InteractionSystemJob : IJobForEach<Translation, Rotation>
    {
        public void Execute(ref Translation translation, [ReadOnly] ref Rotation rotation)
        {
            
            
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new InteractionSystemJob();
        
        return job.Schedule(this, inputDependencies);
    }
}
