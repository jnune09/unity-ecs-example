using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

// @update! @bug!
public class MoveToSystem : JobComponentSystem
{
    struct MoveToSystemJob : IJobForEachWithEntity<Destination, Translation, Direction>
    {
        public float deltaTime;

        public EntityCommandBuffer.Concurrent buffer;

        public void Execute(Entity entity, int index,
            [ReadOnly] ref Destination destination,
            [ReadOnly] ref Translation translation,
            ref Direction direction
            )
        {

            if (math.distance(translation.Value, destination.Value) > 1f)
            {
                direction.Value = destination.Value - translation.Value;
            }
            else
            {
                direction.Value = float3.zero;
                buffer.RemoveComponent<Destination>(index, entity);
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
        var job = new MoveToSystemJob
        {
            deltaTime = Time.DeltaTime,
            buffer = endSimulationBuffer.CreateCommandBuffer().ToConcurrent()
        };

        var jobHandle = job.Schedule(this, inputDeps);

        endSimulationBuffer.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}
