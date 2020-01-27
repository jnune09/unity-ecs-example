using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
//using static Unity.Mathematics.math;

// @update!
public class ConveyorSystem : JobComponentSystem
{
    struct ConveyorSystemJob : IJobForEachWithEntity<Conveyor, Translation>
    {
        public float deltaTime;

        public EntityCommandBuffer.Concurrent buffer;
        [ReadOnly] public ComponentDataFromEntity<Velocity> velocityData;

        public void Execute(Entity entity, int index, ref Conveyor conveyor, ref Translation translation)
        {
            if (!velocityData.Exists(conveyor.Entity))
            {
                return;
            }

            translation.Value += velocityData[conveyor.Entity].Value * deltaTime;

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
        var job = new ConveyorSystemJob
        {
            deltaTime = UnityEngine.Time.deltaTime,

            buffer = endSimulationBuffer.CreateCommandBuffer().ToConcurrent(),
            velocityData = GetComponentDataFromEntity<Velocity>(true)
        };

        var jobHandle = job.Schedule(this, inputDeps);

        endSimulationBuffer.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}
