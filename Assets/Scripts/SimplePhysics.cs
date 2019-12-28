using Unity.Mathematics;
using Unity.Physics;

public static class SimplePhysics
{
    public static bool Intersection(float3x2 box1, float3x2 box2)
    {
        return (box1.c0.x <= box2.c1.x && box1.c1.x >= box2.c0.x - 1f) &&
               (box1.c0.y <= box2.c1.y && box1.c1.y >= box2.c0.y);
    }
    public static bool4 Collision(float3x2 box1, float3x2 box2)
    {
        float tc = math.abs(box1.c1.y - box2.c0.y);
        float bc = math.abs(box2.c1.y - box1.c0.y);
        float lc = math.abs(box1.c0.x - box2.c1.x);
        float rc = math.abs(box2.c0.x - box1.c1.x);

        return new bool4
        {
            x = tc < bc && tc < lc && tc < rc,
            y = bc < tc && bc < lc && bc < rc,
            z = lc < rc && lc < tc && lc < bc,
            w = rc < lc && rc < tc && rc < bc
        };
    }
}
