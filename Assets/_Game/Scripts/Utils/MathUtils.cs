
using UnityEngine;

public static class MathUtils
{
    public static (Vector2 p1, Vector2 p2) GetRectPoints(Vector2 size, Vector2 origin, Vector2 direction)
    {
        (Vector2 p1, Vector2 p2) rect = new();
        Vector2 perpendicular = Vector2.Perpendicular(direction);
        rect.p1 = origin + perpendicular * (size.x / 2);
        rect.p2 = rect.p1 + (direction * size.y) + (-perpendicular * size.x);

        return rect;
    }
}