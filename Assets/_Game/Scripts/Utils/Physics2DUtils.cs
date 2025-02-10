
using System.Collections.Generic;
using UnityEngine;

public static class Physics2DUtils
{
    public static List<T> GetCircleHits<T>(Vector2 point, float radius, int layerMask = -5, float minDepth = float.NegativeInfinity)
        where T : MonoBehaviour
    {
        var hits = Physics2D.OverlapCircleAll(point, radius, layerMask, minDepth);

        return CheckHits<T>(hits);
    }
    public static List<T> GetRectHits<T>(Vector2 p1, Vector2 p2, int layerMask = -5, float minDepth = float.NegativeInfinity)
        where T : MonoBehaviour
    {
        var hits = Physics2D.OverlapAreaAll(p1, p2, layerMask, minDepth);
        return CheckHits<T>(hits);
    }


    private static List<T> CheckHits<T>(Collider2D[] hits)
        where T : MonoBehaviour
    {
        var hitsResult = new List<T>();

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<T>(out var creature))
            {
                hitsResult.Add(creature);
            }
        }

        return hitsResult;
    }
}

