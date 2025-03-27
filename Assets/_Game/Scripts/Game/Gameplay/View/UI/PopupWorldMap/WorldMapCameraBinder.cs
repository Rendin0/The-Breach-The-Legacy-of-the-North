
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class WorldMapCameraBinder : MonoBehaviour
{
    private Camera _camera;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
}