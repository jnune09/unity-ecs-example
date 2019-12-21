using Unity.Mathematics;
using Unity.Physics;

public static class SimplePhysics
{
    public static bool Intersect(AABB box1, AABB box2)
    {
        return (box1.Min.x <= box2.Max.x && box1.Max.x >= box2.Min.x) &&
               (box1.Min.y <= box2.Max.y && box1.Max.y >= box2.Min.y);
    }
}
