
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapCollider2D))]
public class TransparentTilemap : MonoBehaviour
{
    private TilemapCollider2D _collider;
    private Tilemap _tilemap;
    private void Awake()
    {
        _collider = GetComponent<TilemapCollider2D>();
        _tilemap = GetComponent<Tilemap>();

        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Скрытие объекта, если коллизия с игроком
        if (LayerMask.LayerToName(collision.gameObject.layer) == Factions.Player.ToString())
        {
            ToggleTransparency();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == Factions.Player.ToString())
        {
            ToggleTransparency();
        }
    }

    private void ToggleTransparency()
    {
        _tilemap.color = _tilemap.color.a == 0f ? Color.white : Color.clear;
    }

}
