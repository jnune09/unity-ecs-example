using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class PlayerControlSystem : JobComponentSystem
{
    struct PlayerControlSystemJob : IJobForEach<PlayerInput, Direction, InteractBox>
    {

        public void Execute(
            [ReadOnly] ref PlayerInput playerInput,
            ref Direction direction,
            ref InteractBox interactiveBox
            )
        {

            //----------------------------------------------------------------------------------------------------//
            //----------------------------------------------------------------------------------------------------//
            //----------------------------------------------------------------------------------------------------//

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

            //----------------------------------------------------------------------------------------------------//
            //----------------------------------------------------------------------------------------------------//
            //----------------------------------------------------------------------------------------------------//

            if (playerInput.Interact)
            {
                interactiveBox.Active = true;
            }
            else
            {
                interactiveBox.Active = false;
            }

            //----------------------------------------------------------------------------------------------------//
            //----------------------------------------------------------------------------------------------------//
            //----------------------------------------------------------------------------------------------------//
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new PlayerControlSystemJob();
        
        return job.Schedule(this, inputDeps);
    }
}
