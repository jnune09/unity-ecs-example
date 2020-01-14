using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class HitBoxSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = Entities.ForEach((ref HitBox hitBox, in Translation translation) =>
        {
            hitBox.Bounds.Min = translation.Value + hitBox.Position;
            hitBox.Bounds.Max = hitBox.Bounds.Min + hitBox.Size;

        }).Schedule(inputDeps);

        return job;
    }
}
