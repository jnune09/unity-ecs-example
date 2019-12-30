using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class MovementSystem : JobComponentSystem
{   
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        var job = Entities.ForEach((ref Translation translation, in Velocity velocity) =>
        {
            translation.Value += velocity.Value * deltaTime;

        }).Schedule(inputDeps);

        return job;
    }
}
