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

    [BurstCompile]
    [RequireComponentTag(typeof(ActorTag))]
    [ExcludeComponent(typeof(Target))]
    struct TargetSystemJob : IJobForEachWithEntity<Translation>
    {
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<TargetWithPosition> targetWithPositionArray;
        public EntityCommandBuffer.Concurrent entityCommandBuffer;

        public void Execute(Entity entity, int index, [ReadOnly] ref Translation translation)
        {
            float3 position = translation.Value;
            Entity target = Entity.Null;
            float3 targetPosition = float3.zero;

            for (int i =0; i < targetWithPositionArray.Length; i++)
            {
                TargetWithPosition targetWithPosition = targetWithPositionArray[i];

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
                entityCommandBuffer.AddComponent(index, entity, new Target { Entity = target });
            }
        }
    }

    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBuffer;
    protected override void OnCreate()
    {
        endSimulationEntityCommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        base.OnCreate();   
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps) 
    {
        EntityQuery m_targetQuery = GetEntityQuery(typeof(PlayerTag), ComponentType.ReadOnly<Translation>());

        NativeArray<Entity> targetEntityArray = m_targetQuery.ToEntityArray(Allocator.TempJob);
        NativeArray<Translation> targetPositionArray = m_targetQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

        NativeArray<TargetWithPosition> targetWithPositionArray = new NativeArray<TargetWithPosition>(targetEntityArray.Length, Allocator.TempJob);

        for (int i = 0; i < targetEntityArray.Length; i++)
        {
            targetWithPositionArray[i] = new TargetWithPosition
            {
                target = targetEntityArray[i],
                position = targetPositionArray[i].Value
            };
        }

        targetEntityArray.Dispose();
        targetPositionArray.Dispose();

        var job = new TargetSystemJob
        {
            targetWithPositionArray = targetWithPositionArray,
            entityCommandBuffer = endSimulationEntityCommandBuffer.CreateCommandBuffer().ToConcurrent()
        };

        var jobHandle = job.Schedule(this, inputDeps);

        endSimulationEntityCommandBuffer.AddJobHandleForProducer(jobHandle);

        return jobHandle;

    }
}
