using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct PickUp : IComponentData
{
    public Entity Value;
}
