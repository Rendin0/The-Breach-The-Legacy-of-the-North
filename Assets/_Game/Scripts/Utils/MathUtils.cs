
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static List<Vector2> GetRectPoints(Vector2 size, Vector2 origin, Vector2 direction)
    {
        List<Vector2> points = new();
        Vector2 perpendicular = Vector2.Perpendicular(direction);
        points.Add(origin + perpendicular * (size.x / 2));
        points.Add(points[^1] - perpendicular * size.x);
        points.Add(points[^1] + direction * size.y);
        points.Add(points[^1] + perpendicular * size.x);
        return points;
    }
}