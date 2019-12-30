using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
public class PlayerInputSystem : JobComponentSystem
{
    struct PlayerInputSystemJob : IJobForEach<PlayerInput>
    {
        public bool moveUp;
        public bool moveDown;
        public bool moveLeft;
        public bool moveRight;
        public bool interact;

        public void Execute(ref PlayerInput playerInput)
        {
            // movement
            playerInput.MoveUp = moveUp;
            playerInput.MoveDown = moveDown;
            playerInput.MoveLeft = moveLeft;
            playerInput.MoveRight = moveRight;
            // action
            playerInput.Interact = interact;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new PlayerInputSystemJob()
        {
            moveUp = UnityEngine.Input.GetKey(UnityEngine.KeyCode.W),
            moveDown = UnityEngine.Input.GetKey(UnityEngine.KeyCode.S),
            moveLeft = UnityEngine.Input.GetKey(UnityEngine.KeyCode.A),
            moveRight = UnityEngine.Input.GetKey(UnityEngine.KeyCode.D),
            interact = UnityEngine.Input.GetKey(UnityEngine.KeyCode.E),
        };

        return job.Schedule(this, inputDeps);
    }
}
