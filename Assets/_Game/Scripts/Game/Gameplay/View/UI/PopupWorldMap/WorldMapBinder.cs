
using System;
using UnityEngine;

public class WorldMapBinder : MonoBehaviour
{
    [SerializeField] private GameObject _worldLevel;
    [SerializeField] private GameObject _reliefLevel;
    [SerializeField] private GameObject _locationLevel;
    [SerializeField] private GameObject _townLevel;
    public void SetScale(float scale)
    {
        if (scale >= .9f)
            SetWorldScale();
        else if (scale >= .4f)
            SetReliefScale();
        else if (scale >= .2f)
            SetLocationScale();
        else
            SetTownScale();
    }
    private void SetWorldScale()
    {
        _worldLevel.SetActive(true);
        _reliefLevel.SetActive(false);
        _locationLevel.SetActive(false);
        _townLevel.SetActive(false);
    }
    private void SetReliefScale()
    {
        _worldLevel.SetActive(false);
        _reliefLevel.SetActive(true);
        _locationLevel.SetActive(false);
        _townLevel.SetActive(false);
    }
    private void SetLocationScale()
    {
        _worldLevel.SetActive(false);
        _reliefLevel.SetActive(true);
        _locationLevel.SetActive(true);
        _townLevel.SetActive(false);
    }
    private void SetTownScale()
    {
        _worldLevel.SetActive(false);
        _reliefLevel.SetActive(true);
        _locationLevel.SetActive(false);
        _townLevel.SetActive(true);
    }
}