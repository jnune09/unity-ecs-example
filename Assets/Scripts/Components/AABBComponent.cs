using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct AABB : IComponentData
{
    public float3 Min;
    public float3 Max;
}
