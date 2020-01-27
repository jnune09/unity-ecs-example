using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

// @update! @bug!
public class FollowSystem : JobComponentSystem
{
    struct FollowSystemJob : IJobForEach<Follow, Translation, Direction>
    {
        public float deltaTime;
        [ReadOnly] public ComponentDataFromEntity<Translation> translationData;
        
        public void Execute(
            [ReadOnly] ref Follow follow, 
            [ReadOnly] ref Translation translation, 
            ref Direction direction
            )
        {
            if (!translationData.Exists(follow.Entity))
            {
                return;
            }

            Translation targetTranslation = translationData[follow.Entity];

            if (math.distance(translation.Value, targetTranslation.Value) > 16f)
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
        var job = new FollowSystemJob
        {
            deltaTime = UnityEngine.Time.deltaTime,
            translationData = GetComponentDataFromEntity<Translation>(true) 
        };

        return job.Schedule(this, inputDeps);
    }
}
