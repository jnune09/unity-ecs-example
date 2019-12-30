using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

public class HungerSystem : JobComponentSystem
{   
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        var job = Entities.ForEach((ref Hunger hunger) =>
        {
            hunger.Value += 1f * deltaTime;

        }).Schedule(inputDeps);
        
        return job;
    }
}
