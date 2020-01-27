using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class CarrySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = Entities.ForEach((ref Carry carry, ref DynamicBuffer<Inventory> inventory) =>
        {
            if (carry.Entity != Entity.Null)
            {
                inventory.Add(new Inventory { Item = carry.Entity, Count = 1 });
                carry.Entity = Entity.Null;
            }

        }).Schedule(inputDeps);

        return job;
    }
}
