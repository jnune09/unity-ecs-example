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
            float3 reverse = new float3(0, 0, 0);

            if (collision.Top)
            {
                reverse.y = -1f;
            }
            if (collision.Bottom)
            {
                reverse.y = 1f;
            }
            if (collision.Left)
            {
                reverse.x = 1f;
            }
            if (collision.Right)
            {
                reverse.x = -1f;
            }

            //float3 scale = new float3(1,1,0);
            //if (collision.Top && collision.Bottom)
            //{
            //    scale.y = 0;
            //}
            //if (collision.Left && collision.Right)
            //{
            //    scale.x = 0;
            //}

            //velocity.Value = math.normalizesafe(math.normalizesafe(direction.Value) + reverse) * scale * speed.Value;

            velocity.Value = math.normalizesafe(math.normalizesafe(direction.Value) + reverse) * speed.Value;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new VelocitySystemJob();
        
        return job.Schedule(this, inputDependencies);
    }
}
