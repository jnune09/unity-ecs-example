using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct TargetBox : IComponentData
{
    public float Offset;
    public float Distance;
    public float3 Size;
    public float3x2 Value;
}
