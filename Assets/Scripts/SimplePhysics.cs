using Unity.Mathematics;

public static class SimplePhysics
{
    public static bool Intersection(AABB box1, AABB box2)
    {
        return (box1.Min.x <= box2.Max.x && box1.Max.x >= box2.Min.x) &&
               (box1.Min.y <= box2.Max.y && box1.Max.y >= box2.Min.y);
    }
    public static bool4 Collision(AABB box1, AABB box2)
    {
        float tc = math.abs(box1.Max.y - box2.Min.y);
        float bc = math.abs(box2.Max.y - box1.Min.y);
        float lc = math.abs(box1.Min.x - box2.Max.x);
        float rc = math.abs(box2.Min.x - box1.Max.x);

        return new bool4
        {
            x = tc < bc && tc < lc && tc < rc,
            y = bc < tc && bc < lc && bc < rc,
            z = lc < rc && lc < tc && lc < bc,
            w = rc < lc && rc < tc && rc < bc
        };
    }
}
