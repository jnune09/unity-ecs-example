using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

// @update!
public class ActionSystem : JobComponentSystem
{
    struct ActionSystemJob : IJobForEachWithEntity<ActionBox, Translation>
    {
        public EntityCommandBuffer.Concurrent buffer;
        [ReadOnly] public ComponentDataFromEntity<Item> itemData;
        public void Execute(Entity entity, int index, [ReadOnly] ref ActionBox actionBox, [ReadOnly] ref Translation translation)
        {
            if (actionBox.CoActor != Entity.Null && actionBox.ActionID != 0)
            {
                if (actionBox.ActionID == 1)
                {
                    buffer.AddComponent(index, actionBox.CoActor, new Damage { Value = 5f });
                    buffer.AddComponent(index, actionBox.CoActor, new Follow { Entity = entity });
                }
                if (actionBox.ActionID == 2)
                {
                    if(itemData.Exists(actionBox.CoActor))
                    {
                        Entity item = buffer.CreateEntity(index);
                        buffer.AddComponent(index, item, itemData[actionBox.CoActor]);
                        buffer.AddComponent(index, entity, new Carry { Entity = item });
                        buffer.DestroyEntity(index, actionBox.CoActor);
                    }
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
            buffer = endSimulationBuffer.CreateCommandBuffer().ToConcurrent(),
            itemData = GetComponentDataFromEntity<Item>(true)
        };

        var jobHandle = job.Schedule(this, inputDeps);

        endSimulationBuffer.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}
