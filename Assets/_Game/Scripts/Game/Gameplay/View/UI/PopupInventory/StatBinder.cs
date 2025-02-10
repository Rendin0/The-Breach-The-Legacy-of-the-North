using R3;
using TMPro;
using UnityEngine;

public class StatBinder : MonoBehaviour
{
    [SerializeField] TMP_Text _value;
    private readonly CompositeDisposable _subs = new();

    private void OnDestroy()
    {
        _subs.Dispose();
    }

    public void Bind(Observable<float> stat)
    {
        stat.Subscribe(v => _value.text = v.ToString()).AddTo(_subs);
    }
}