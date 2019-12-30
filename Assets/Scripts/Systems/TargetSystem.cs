using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class TargetSystem : JobComponentSystem
{
    private struct TargetWithPosition
    {
        public Entity target;
        public float3 position;
    }

    [RequireComponentTag(typeof(Interaction))]
    [ExcludeComponent(typeof(Target))]
    // split this into a target and follow system
    struct TargetSystemJob : IJobForEachWithEntity<Translation>
    {
        // create the array they will be created with a query
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<TargetWithPosition> targetArray;

        public EntityCommandBuffer.Concurrent buffer;

        public void Execute(Entity entity, int index,
            [ReadOnly] ref Translation translation
            )
        {
            var position = translation.Value;
            var target = Entity.Null;
            var targetPosition = float3.zero;

            for (int i =0; i < targetArray.Length; i++)
            {
                TargetWithPosition targetWithPosition = targetArray[i];

                if (target == Entity.Null)
                {
                    // no target
                    target = targetWithPosition.target;
                    targetPosition = targetWithPosition.position;
                }
                else
                {
                    if (math.distance(position, targetWithPosition.position) < math.distance(position, targetPosition))
                    {
                        // closer target
                        target = targetWithPosition.target;
                        targetPosition = targetWithPosition.position;

                    }
                }
            }

            if (target != Entity.Null)
            {
                buffer.AddComponent(index, entity, new Target { Entity = target });
            }

        }
    }

    private EndSimulationEntityCommandBufferSystem endSimulationBuffer;
    private EntityQuery m_targetQuery;

    protected override void OnCreate()
    {
        endSimulationBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        m_targetQuery = GetEntityQuery(typeof(PlayerTag), ComponentType.ReadOnly<Translation>());
        base.OnCreate();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps) 
    {
        var targetEntityArray = m_targetQuery.ToEntityArray(Allocator.TempJob);
        var targetPositionArray = m_targetQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

        var targetArray = new NativeArray<TargetWithPosition>(targetEntityArray.Length, Allocator.TempJob);

        for (int i = 0; i < targetEntityArray.Length; i++)
        {
            targetArray[i] = new TargetWithPosition
            {
                target = targetEntityArray[i],
                position = targetPositionArray[i].Value
            };
        }

        targetEntityArray.Dispose();
        targetPositionArray.Dispose();

        var job = new TargetSystemJob
        {
            targetArray = targetArray,
            buffer = endSimulationBuffer.CreateCommandBuffer().ToConcurrent()
        };

        var jobHandle = job.Schedule(this, inputDeps);

        endSimulationBuffer.AddJobHandleForProducer(jobHandle);

        return jobHandle;

    }
}
