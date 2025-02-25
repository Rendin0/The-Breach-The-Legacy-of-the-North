
using UnityEngine;
using UnityEngine.UI;

public class NonRectButton : Button
{
    [SerializeField] private float _alphaThreshold = .1f;

    protected override void Awake()
    {
        base.Awake();
        var img = (Image)targetGraphic;
        img.alphaHitTestMinimumThreshold = _alphaThreshold;
    }
}