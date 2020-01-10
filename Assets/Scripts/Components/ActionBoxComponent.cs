using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct ActionBox : IComponentData
{
    public Entity CoActor;
    public int ActionID;
    public float Distance;
    public float Offset;
    public float3 Direction;
    public float3 Size;
    public float3x2 Value;
}
