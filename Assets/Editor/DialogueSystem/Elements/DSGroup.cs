using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DSGroup : Group
{
    private Color _defaultColor;
    private float _defaultBorderWidth;

    public string Id { get; set; }
    public string OldTitle { get; set; }
    public DSGroup(string groupTitle, Vector2 position)
    {
        Id = Guid.NewGuid().ToString();

        title = OldTitle = groupTitle;
        SetPosition(new Rect(position, Vector2.zero));

        _defaultColor = contentContainer.style.borderBottomColor.value;
        _defaultBorderWidth = contentContainer.style.borderBottomWidth.value;
    }

    public void SetErrorStyle(Color color)
    {
        contentContainer.style.borderBottomColor = color;
        contentContainer.style.borderBottomWidth = 2f;
    }

    public void ResetStyles()
    {

        contentContainer.style.borderBottomColor = _defaultColor;
        contentContainer.style.borderBottomWidth = _defaultBorderWidth;
    }
}
