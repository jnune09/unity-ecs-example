using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class ActionSystem : JobComponentSystem
{
    [BurstCompile]
    struct ActionSystemJob : IJobForEachWithEntity<ActionBox>
    {
        public EntityCommandBuffer.Concurrent buffer;
        public void Execute(Entity entity, int index, [ReadOnly] ref ActionBox actionBox)
        {
            if (actionBox.CoActor != Entity.Null && actionBox.ActionID != 0)
            {
                if (actionBox.ActionID == 1)
                {
                    buffer.AddComponent(index, actionBox.CoActor, new Damage { Value = 5f });
                }
            }
        }
    }

    private EndSimulationEntityCommandBufferSystem endSimulationBuffer;

    protected override void OnCreate()
    {
        // get the end sim buffer from world
        endSimulationBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        base.OnCreate();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new ActionSystemJob
        {
            buffer = endSimulationBuffer.CreateCommandBuffer().ToConcurrent()
        };

        var jobHandle = job.Schedule(this, inputDeps);

        endSimulationBuffer.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}
