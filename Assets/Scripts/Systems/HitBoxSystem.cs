using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class HitBoxSystem : JobComponentSystem
{
    struct HitBoxSystemJob : IJobForEach<Translation, HitBox>
    {
        public void Execute(
            [ReadOnly] ref Translation translation,
            ref HitBox hitBox
            )
        {
            hitBox.Value.c0 = translation.Value + hitBox.Position;
            hitBox.Value.c1 = hitBox.Value.c0 + hitBox.Size;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new HitBoxSystemJob();
        
        return job.Schedule(this, inputDeps);
    }
}
