using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class VelocitySystem : JobComponentSystem
{   
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = Entities.ForEach((ref Velocity velocity, in Collision collision, in Direction direction, in Speed speed) =>
        {
            velocity.Value = math.normalizesafe(math.normalizesafe(direction.Value) + collision.Direction) * speed.Value;

        }).Schedule(inputDeps);

        return job;
    }
}
