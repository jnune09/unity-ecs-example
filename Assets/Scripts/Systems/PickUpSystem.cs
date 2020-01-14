using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

// @update!
public class PickUpSystem : JobComponentSystem
{
    struct PickUpSystemJob : IJobForEachWithEntity<Item, PickUp>
    {
        [ReadOnly] public BufferFromEntity<Inventory> inventoryData;
        public EntityCommandBuffer.Concurrent buffer;

        public void Execute(Entity entity, int index, [ReadOnly] ref Item item, ref PickUp pickUp)
        {
            //inventoryData[pickUp.Value].Add(new Inventory { Item = entity, Count = 1 });
            //buffer.AddComponent<Item>(index, pickUp.Value, item);
            buffer.DestroyEntity(index, entity);
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
        var job = new PickUpSystemJob
        {
            buffer = endSimulationBuffer.CreateCommandBuffer().ToConcurrent(),
            inventoryData = GetBufferFromEntity<Inventory>(true)
        };

        var jobHandle = job.Schedule(this, inputDeps);

        endSimulationBuffer.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}
