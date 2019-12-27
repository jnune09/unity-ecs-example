using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct Collision : IComponentData
{
    public bool Top;
    public bool Bottom;
    public bool Left;
    public bool Right;
}
