using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class InteractionSystem : JobComponentSystem
{

    //----------------------------------------------------------------------------------------------------//
    //----------------------------------------------------------------------------------------------------//
    //----------------------------------------------------------------------------------------------------//

    // create a data type to store individual query results
    private struct Actor
    {
        public Entity actor;
        public float3x2 hitBox;
    }

    [ExcludeComponent(typeof(Interaction))]
    struct InteractionSystemJob : IJobForEachWithEntity<InteractBox>
    {
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<Actor> actorArray;
        public EntityCommandBuffer.Concurrent buffer;

        public void Execute(Entity entity, int index,
            [ReadOnly] ref InteractBox interactBox
            )
        {
            // add interaction component if the interact box is active and intersects with an actor's hitbox
            for (int i = 0; i < actorArray.Length; i++)
            {
                if (SimplePhysics.Intersection(interactBox.Value, actorArray[i].hitBox))
                {
                    if (interactBox.Active)
                    {
                        buffer.AddComponent(index, actorArray[i].actor, new Interaction { });
                    }
                }
            }

        }
    }

    //----------------------------------------------------------------------------------------------------//
    //----------------------------------------------------------------------------------------------------//
    //----------------------------------------------------------------------------------------------------//

    private EndSimulationEntityCommandBufferSystem endSimulationBuffer;
    private EntityQuery m_actorQuery;

    protected override void OnCreate()
    {
        // get the end sim buffer from world
        endSimulationBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        // query for entities with actortag and hitbox, with read only permission on hitbox
        m_actorQuery = GetEntityQuery(typeof(ActorTag), ComponentType.ReadOnly<HitBox>());

        base.OnCreate();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        // store query into seperate arrays
        var entityArray = m_actorQuery.ToEntityArray(Allocator.TempJob);
        var hitBoxArray = m_actorQuery.ToComponentDataArray<HitBox>(Allocator.TempJob);

        // populate actor array with query results
        var actorArray = new NativeArray<Actor>(entityArray.Length, Allocator.TempJob);
        for (int i = 0; i < entityArray.Length; i++)
        {
            actorArray[i] = new Actor
            {
                actor = entityArray[i],
                hitBox = hitBoxArray[i].Value
            };
        }

        // dispose of the query arrays, actor array will dispose itself after the job is done
        entityArray.Dispose();
        hitBoxArray.Dispose();

        // pass actor array and buffer to the job
        var job = new InteractionSystemJob
        {
            actorArray = actorArray,
            buffer = endSimulationBuffer.CreateCommandBuffer().ToConcurrent()
        };

        var jobHandle = job.Schedule(this, inputDeps);

        endSimulationBuffer.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }

}
