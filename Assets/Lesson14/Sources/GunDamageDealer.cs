using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDamageDealer : MonoBehaviour
{
    public event Action<int> OnHit;

    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private SphereCast _gun;
    [SerializeField] private int _damage;

    public SphereCast Gun { get { return _gun; } }

    // Start is called before the first frame update
    void Start()
    {
        _gun.OnHit += GunHitHandler;
    }

    // Update is called once per frame
    private void GunHitHandler(Collider collider)
    {
        if(_healthSystem.GetHealth(collider, out Health health))
        {
            health.TakeDamage(_damage);
        }

        OnHit?.Invoke(health ? 1 : 0);
    }
}
