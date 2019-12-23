using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MovementSystem : JobComponentSystem
{

    [BurstCompile]
    struct MovementSystemJob : IJobForEach<Translation, Velocity>
    {
        public float deltaTime;
        public void Execute(ref Translation translation, [ReadOnly] ref Velocity velocity)
        {
            translation.Value += math.normalizesafe(velocity.Direction) * velocity.Speed * deltaTime;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new MovementSystemJob()
        {
            deltaTime = UnityEngine.Time.deltaTime
        };

        return job.Schedule(this, inputDeps);
    }
}
