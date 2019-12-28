using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class FocusBoxSystem : JobComponentSystem
{
    [BurstCompile]
    struct FocusBoxSystemJob : IJobForEach<Direction, Translation, TargetBox>
    {

        public void Execute(
            [ReadOnly] ref Direction direction,
            [ReadOnly] ref Translation translation,
            ref TargetBox targetBox
            )
        {
            targetBox.Value.c0 = (direction.Value * targetBox.Distance) + targetBox.Offset;
            targetBox.Value.c1 = targetBox.Value.c0 + targetBox.Size;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new FocusBoxSystemJob();
        
        return job.Schedule(this, inputDependencies);
    }
}
