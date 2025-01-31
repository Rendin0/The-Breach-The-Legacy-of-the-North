using System.Collections.Generic;
using UnityEngine;

public class DSNodeErrorData
{
    public DSErrorData ErrorData { get; set; } = new();
    public List<DSNode> Nodes {  get; set; } = new();
}
