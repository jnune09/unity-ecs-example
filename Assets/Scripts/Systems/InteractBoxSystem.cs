using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class InteractBoxSystem : JobComponentSystem
{   
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = Entities.ForEach((ref InteractBox interactBox, in Direction direction, in Translation translation) =>
        {
            if ((interactBox.Direction.x != direction.Value.x && direction.Value.x != 0) ||
                (interactBox.Direction.y != direction.Value.y && direction.Value.y != 0))
            {
                interactBox.Direction = direction.Value;
            }

            interactBox.Value.c0 = translation.Value + (interactBox.Direction * interactBox.Distance + interactBox.Offset);
            interactBox.Value.c1 = interactBox.Value.c0 + interactBox.Size;

        }).Schedule(inputDeps);

        return job;
    }
}
