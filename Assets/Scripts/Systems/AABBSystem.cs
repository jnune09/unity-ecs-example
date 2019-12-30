using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class AABBSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = Entities.ForEach((ref AABB aabb, in Translation translation) =>
        {
            aabb.Value.c0 = translation.Value + aabb.Position;
            aabb.Value.c1 = aabb.Value.c0 + aabb.Size;

        }).Schedule(inputDeps);

        return job;
    }
}
