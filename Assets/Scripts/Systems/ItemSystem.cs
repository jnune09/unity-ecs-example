using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
//using static Unity.Mathematics.math;

// @update!
public class ItemSystem : JobComponentSystem
{
    struct ItemSystemJob : IJobForEachWithEntity<Item>
    {
        public float deltaTime;

        public EntityCommandBuffer.Concurrent buffer;

        public void Execute(Entity entity, int index, ref Item item)
        {

        }
    }

    private EndSimulationEntityCommandBufferSystem endSimulationBuffer;

    protected override void OnCreate()
    {
        endSimulationBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        base.OnCreate();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new ItemSystemJob
        {
            deltaTime = UnityEngine.Time.deltaTime,

            buffer = endSimulationBuffer.CreateCommandBuffer().ToConcurrent(),
        };

        var jobHandle = job.Schedule(this, inputDeps);

        endSimulationBuffer.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}
