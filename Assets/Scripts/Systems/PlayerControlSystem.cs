using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class PlayerControlSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = Entities.ForEach((ref ActionBox actionBox, ref Direction direction, in PlayerInput playerInput) =>
        {
            direction.Value = float3.zero;

            if (playerInput.MoveUp)
            {
                direction.Value.y += 1f;
            }
            if (playerInput.MoveDown)
            {
                direction.Value.y -= 1f;
            }
            if (playerInput.MoveLeft)
            {
                direction.Value.x -= 1f;
            }
            if (playerInput.MoveRight)
            {
                direction.Value.x += 1f;
            }

            if (playerInput.Attack)
            {
                actionBox.ActionID = 1;
            }
            else
            {
                actionBox.ActionID = 0;
            }

        }).Schedule(inputDeps);

        return job;
    }
}
