using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct Carry : IComponentData
{
    public Entity Entity;
}
