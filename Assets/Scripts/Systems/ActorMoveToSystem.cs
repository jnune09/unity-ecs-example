using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class ActorMoveToSystem : JobComponentSystem
{

    //[BurstCompile]
    struct ActorMoveToSystemJob : IJobForEach<Target, Translation, Velocity>
    {
        public float deltaTime;
        [ReadOnly] public ComponentDataFromEntity<Translation> translationData;
        
        public void Execute([ReadOnly] ref Target target, [ReadOnly] ref Translation translation, ref Velocity velocity)
        {
            if (!translationData.Exists(target.Entity))
            {
                return;
            }

            Translation targetTranslation = translationData[target.Entity];

            if (math.distance(translation.Value, targetTranslation.Value) > 1f)
            {
                velocity.Direction = math.normalize(targetTranslation.Value - translation.Value);
            }
            else
            {
                velocity.Direction = Unity.Mathematics.float3.zero;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new ActorMoveToSystemJob
        {
            deltaTime = UnityEngine.Time.deltaTime,
            translationData = GetComponentDataFromEntity<Translation>(true) 
        };

        return job.Schedule(this, inputDeps);
    }
}
