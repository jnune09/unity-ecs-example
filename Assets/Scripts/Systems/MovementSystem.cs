using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class MovementSystem : JobComponentSystem
{

    [BurstCompile]
    struct MovementSystemJob : IJobForEach<PlayerTag, Translation, MoveSpeedComponent>
    {

        public float deltaTime;
        public bool up;
        public bool down;
        public bool left;
        public bool right;

        public void Execute([ReadOnly] ref PlayerTag playerTag, ref Translation translation, [ReadOnly] ref MoveSpeedComponent moveSpeed)
        {
            if (up)
                translation.Value.y += moveSpeed.Value * deltaTime;
            if (down)
                translation.Value.y -= moveSpeed.Value * deltaTime;
            if (left)
                translation.Value.x -= moveSpeed.Value * deltaTime;
            if (right)
                translation.Value.x += moveSpeed.Value * deltaTime;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new MovementSystemJob();

        job.deltaTime = UnityEngine.Time.deltaTime;
        job.up = UnityEngine.Input.GetKey(UnityEngine.KeyCode.W);
        job.down = UnityEngine.Input.GetKey(UnityEngine.KeyCode.S);
        job.left = UnityEngine.Input.GetKey(UnityEngine.KeyCode.A);
        job.right = UnityEngine.Input.GetKey(UnityEngine.KeyCode.D);

        return job.Schedule(this, inputDependencies);
    }
}
