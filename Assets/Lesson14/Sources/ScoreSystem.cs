using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public event Action OnDataUpdated;

    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private GunDamageDealer _gunDamageDealer;

    private Vector3Int _kda;
    private int _shotCount;
    private int _hitCount;

    public Vector3Int KDA => _kda;
    public int Accuracy => _shotCount == 0f ? 0 : (int)((_hitCount / (float)_shotCount) * 100f);

    // Start is called before the first frame update
    void Start()
    {
        _healthSystem.OnCharacterDeath += CharacterDeathHandler;
        _gunDamageDealer.OnHit += HitHendler;
        _gunDamageDealer.Gun.OnShot += ShotHandler;
    }

    private void CharacterDeathHandler(Health health)
    {
        _kda.x++;
        OnDataUpdated?.Invoke();
    }

    private void HitHendler(int hits)
    {
        _hitCount += hits;
        OnDataUpdated?.Invoke();
    }

    private void ShotHandler()
    {
        _shotCount++;
        OnDataUpdated?.Invoke();
    }
}
