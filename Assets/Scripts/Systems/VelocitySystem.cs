using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class VelocitySystem : JobComponentSystem
{
    [BurstCompile]
    struct VelocitySystemJob : IJobForEach<Collision, Direction, Speed, Velocity>
    {
        public void Execute(
            [ReadOnly] ref Collision collision,
            [ReadOnly] ref Direction direction,
            [ReadOnly] ref Speed speed,
            ref Velocity velocity
            )
        {

            velocity.Value = math.normalizesafe(math.normalizesafe(direction.Value) + collision.Value) * speed.Value;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new VelocitySystemJob();
        
        return job.Schedule(this, inputDeps);
    }
}
