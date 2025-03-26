

using UnityEngine;

public class GOAPService
{
    private readonly CreaturesSerivce _creaturesSerivce;

    public GOAPService(CreaturesSerivce creaturesSerivce)
    {
        this._creaturesSerivce = creaturesSerivce;
    }

    public int GetPlayerHealth()
    {
        return Mathf.CeilToInt(_creaturesSerivce.GetPlayer().Stats.Health.Value);
    }
}

