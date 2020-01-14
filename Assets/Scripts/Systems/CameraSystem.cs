using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class CameraSystem : JobComponentSystem
{
    [ReadOnly] EntityQuery m_playerQuery;
    protected override void OnCreate()
    {
        m_playerQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerTag>(), ComponentType.ReadOnly<Translation>());

        base.OnCreate();
    }
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var playerArray = m_playerQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

        float3 playerPosition = playerArray[0].Value;

        playerArray.Dispose();

        var job = Entities.ForEach((ref Camera camera) =>
        {
            camera.Position.x = playerPosition.x;
            camera.Position.y = playerPosition.y;

        }).Schedule(inputDependencies);

        return job;
    }
}
