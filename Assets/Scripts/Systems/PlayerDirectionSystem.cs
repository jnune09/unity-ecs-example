using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class PlayerDirectionSystem : JobComponentSystem
{
    [BurstCompile]
    struct PlayerDirectionSystemJob : IJobForEach<PlayerInput, Direction>
    {

        public void Execute(
            [ReadOnly] ref PlayerInput playerInput,
            ref Direction direction
            )
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
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new PlayerDirectionSystemJob();
        
        return job.Schedule(this, inputDependencies);
    }
}
