using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MoveToSystem : JobComponentSystem
{

    [BurstCompile]
    struct MoveToSystemJob : IJobForEach<Target, Translation, Direction>
    {
        public float deltaTime;
        [ReadOnly] public ComponentDataFromEntity<Translation> translationData;
        
        public void Execute(
            [ReadOnly] ref Target target, 
            [ReadOnly] ref Translation translation, 
            ref Direction direction
            )
        {
            if (!translationData.Exists(target.Entity))
            {
                return;
            }

            Translation targetTranslation = translationData[target.Entity];

            if (math.distance(translation.Value, targetTranslation.Value) > 36f)
            {
                direction.Value = targetTranslation.Value - translation.Value;
            }
            else
            {
                direction.Value = float3.zero;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new MoveToSystemJob
        {
            deltaTime = UnityEngine.Time.deltaTime,
            translationData = GetComponentDataFromEntity<Translation>(true) 
        };

        return job.Schedule(this, inputDeps);
    }
}
