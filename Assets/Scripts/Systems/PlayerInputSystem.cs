using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
public class PlayerInputSystem : JobComponentSystem
{
    [BurstCompile]
    struct PlayerInputSystemJob : IJobForEach<PlayerInput>
    {
        public bool moveUp;
        public bool moveDown;
        public bool moveLeft;
        public bool moveRight;

        public void Execute(ref PlayerInput playerInput)
        {
            playerInput.MoveUp = moveUp;
            playerInput.MoveDown = moveDown;
            playerInput.MoveLeft = moveLeft;
            playerInput.MoveRight = moveRight;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new PlayerInputSystemJob()
        {
            moveUp = UnityEngine.Input.GetKey(UnityEngine.KeyCode.W),
            moveDown = UnityEngine.Input.GetKey(UnityEngine.KeyCode.S),
            moveLeft = UnityEngine.Input.GetKey(UnityEngine.KeyCode.A),
            moveRight = UnityEngine.Input.GetKey(UnityEngine.KeyCode.D),
        };

        return job.Schedule(this, inputDependencies);
    }
}
