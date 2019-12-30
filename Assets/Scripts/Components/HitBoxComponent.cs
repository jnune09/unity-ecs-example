using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct HitBox: IComponentData
{
    public float3 Position;
    public float3 Size;
    public float3x2 Value;
}
