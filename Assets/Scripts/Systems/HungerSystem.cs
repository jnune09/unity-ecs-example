using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class HungerSystem : JobComponentSystem
{

    [BurstCompile]
    struct HungerSystemJob : IJobForEachWithEntity<Hunger>
    {
        public float deltaTime;

        public void Execute(Entity entity, int index,
            ref Hunger hunger
            )
        {
            hunger.Value += 1f * deltaTime;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new HungerSystemJob();

        job.deltaTime = UnityEngine.Time.deltaTime;
        
        return job.Schedule(this, inputDeps);
    }
}
