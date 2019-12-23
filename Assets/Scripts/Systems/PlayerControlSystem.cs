using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class PlayerControlSystem : JobComponentSystem
{
    [BurstCompile]
    struct PlayerControlSystemJob : IJobForEach<PlayerInput, Velocity>
    {

        public void Execute([ReadOnly] ref PlayerInput playerInput, ref Velocity velocity)
        {
            float3 direction = Unity.Mathematics.float3.zero;

            if (playerInput.MoveUp)
            {
                direction.y += 1f;
            }
            if (playerInput.MoveDown)
            {
                direction.y -= 1f;
            }
            if (playerInput.MoveLeft)
            {
                direction.x -= 1f;
            }
            if (playerInput.MoveRight)
            {
                direction.x += 1f;
            }
            
            velocity.Direction = direction;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new PlayerControlSystemJob();
        
        return job.Schedule(this, inputDependencies);
    }
}