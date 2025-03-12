
using CrashKonijn.Agent.Core;

public abstract class ActionData : IActionData
{
    public ITarget Target { get; set; }
    public float Timer { get; set; }
}