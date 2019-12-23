using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct PlayerInput : IComponentData
{
    public bool MoveUp;
    public bool MoveDown;
    public bool MoveLeft;
    public bool MoveRight;
}
