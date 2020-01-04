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
            hitBox.Value.c0 = translation.Value + hitBox.Position;
            hitBox.Value.c1 = hitBox.Value.c0 + hitBox.Size;

        }).Schedule(inputDeps);

        return job;
    }
}
