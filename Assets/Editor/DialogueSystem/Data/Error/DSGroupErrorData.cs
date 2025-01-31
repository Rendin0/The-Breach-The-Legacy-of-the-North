
using System.Collections.Generic;

public class DSGroupErrorData
{
    public DSErrorData ErrorData { get; set; } = new();
    public List<DSGroup> Groups { get; set; } = new();
}
