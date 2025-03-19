
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


    public static List<T> GetColliderHits<T>(List<Vector2> points)
        where T : MonoBehaviour
    {
        return GetColliderHits<T>(points, -1);
    }

    public static List<T> GetColliderHits<T>(List<Vector2> points, LayerMask layerMask)
        where T : MonoBehaviour
    {
        var gameObject = new GameObject();
        var collider = gameObject.AddComponent<PolygonCollider2D>();
        var filter = new ContactFilter2D();
        filter.SetLayerMask(layerMask);

        collider.points = points.ToArray();
        var hits = new List<Collider2D>();

        Physics2D.OverlapCollider(collider, filter, hits);

        GameObject.Destroy(gameObject);
        return CheckHits<T>(hits.ToArray());
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

