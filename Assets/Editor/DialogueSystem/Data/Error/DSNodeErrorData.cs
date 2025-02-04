using System.Collections.Generic;

public class DSNodeErrorData
{
    public DSErrorData ErrorData { get; set; } = new();
    public List<DSNode> Nodes { get; set; } = new();
}
