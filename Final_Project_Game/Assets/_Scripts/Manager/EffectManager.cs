using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Plow,
}

public class EffectManager : MonoBehaviour
{
    private static EffectManager _instance;
    public static EffectManager Instance => _instance;

    [SerializeField] private ParticleSystem _plowTileEffect;

    void Awake()
    {
        _instance = this;
    }

    public void CreateEffect(EffectType type, Vector3 position)
    {
        switch (type)
        {
            case EffectType.Plow:
                ParticleSystem particleSystem = Instantiate<ParticleSystem>(_plowTileEffect, null);
                particleSystem.gameObject.transform.position = position;
                break;
        }
    }
}
