using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[InternalBufferCapacity(8)]
public struct Inventory : IBufferElementData
{
    public Entity Item;
    public int Count;
}
