using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

// @update!
public class CollisionSystem : JobComponentSystem
{
    struct CollisionSystemJob : IJobForEachWithEntity<Translation, Collision>
    {
        [ReadOnly] public NativeArray<Collision> colliders;

        public void Execute(Entity entity, int index,
            [ReadOnly] ref Translation translation,
            ref Collision collision
            )
        {
            collision.Bounds.Min = translation.Value + collision.Position;
            collision.Bounds.Max = collision.Bounds.Min + collision.Size;
            
            // reset collision value
            collision.Direction = float3.zero;

            for (int j = 0; j < colliders.Length; j++)
            {
                if ( /* index != j && */ SimplePhysics.Intersection(colliders[index].Bounds, colliders[j].Bounds))
                {
                    bool4 detection = SimplePhysics.Collision(colliders[index].Bounds, colliders[j].Bounds);
                    if (detection.x)
                    {
                        collision.Direction.y = -1f;
                    }
                    if (detection.y)
                    {
                        collision.Direction.y = 1f;
                    }
                    if (detection.z)
                    {
                        collision.Direction.x = 1f;
                    }
                    if (detection.w)
                    {
                        collision.Direction.x = -1f;
                    }
                }
            }

        }
    }

    [ReadOnly] private EntityQuery m_collisionQuery;

    protected override void OnCreate()
    {
        m_collisionQuery = GetEntityQuery(ComponentType.ReadOnly<Collision>());
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var colliders = m_collisionQuery.ToComponentDataArray<Collision>(Allocator.TempJob);

        var job = new CollisionSystemJob
        {
            colliders = colliders
        };

        var jobHandle = job.Schedule(this, inputDeps);

        jobHandle.Complete();

        colliders.Dispose();

        return jobHandle;
    }
}
