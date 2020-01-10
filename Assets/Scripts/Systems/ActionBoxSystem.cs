using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

// in this system we will store any entity that is colliding with the action box in the action box
// if a action is choosen we will also store the action
// in the action system (maybe or maybe not this one) if entity is not null and an action is choosen then an action comp
// will be given to the stored entity which will then run through a system so resolve the action

public class ActionBoxSystem : JobComponentSystem
{

    // create a data type to store individual query results
    private struct Actor
    {
        public Entity actor;
        public float3x2 hitBox;
    }

    [ReadOnly] private EntityQuery m_actorQuery;

    protected override void OnCreate()
    {
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

        var job = Entities.ForEach((ref ActionBox actionBox, in Direction direction, in Translation translation) =>
        {
            if ((actionBox.Direction.x != direction.Value.x && direction.Value.x != 0) ||
                (actionBox.Direction.y != direction.Value.y && direction.Value.y != 0))
            {
                actionBox.Direction = direction.Value;
            }

            actionBox.Value.c0 = translation.Value + (actionBox.Direction * actionBox.Distance + actionBox.Offset);
            actionBox.Value.c1 = actionBox.Value.c0 + actionBox.Size;

            actionBox.CoActor = Entity.Null;
            //hasTarget = false;

            for (int i = 0; i < actorArray.Length; i++)
            {
                if (SimplePhysics.Intersection(actionBox.Value, actorArray[i].hitBox))
                {
                    actionBox.CoActor = actorArray[i].actor;
                    //hasTarget = true;
                }
            }

        }).Schedule(inputDeps);

        job.Complete();

        actorArray.Dispose();

        return job;
    }
}
