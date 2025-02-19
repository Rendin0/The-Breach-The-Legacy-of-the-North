
using R3;

public class Damage
{
    public readonly DamageData Value;

    public ReactiveProperty<float> Magical = new();
    public ReactiveProperty<float> Physical = new();

    public Damage(DamageData origin)
    {
        Value = origin;

        Magical.OnNext(origin.MagicalData);
        Physical.OnNext(origin.PhysicalData);

        Magical.Skip(1).Subscribe(v => origin.MagicalData = v);
        Physical.Skip(1).Subscribe(v => origin.PhysicalData = v);
    }

    public static DamageData operator *(Damage a, float mod)
    {
        return new() { PhysicalData = a.Physical.Value * mod, MagicalData = a.Magical.Value * mod };
    }
}
