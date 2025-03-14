using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private Dummy _enemyPrefab;
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private CameraSystem _cameraSystem;
    [SerializeField] private float _spawnRangeX = 50f;
    [SerializeField] private float _spawnRangeZ = 50f;
    [SerializeField] private int _countEnemy = 5;

    private Vector3 _spawnPoint;

    private YieldInstruction _spawnInstruction;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _countEnemy; i++)
        {
            Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Spawn()
    {
        _spawnPoint = new Vector3(Random.Range(_spawnRangeX - 10, _spawnRangeX + 10), 0, Random.Range(_spawnRangeZ - 10, _spawnRangeZ + 10));
        Dummy dummy = Instantiate(_enemyPrefab, _spawnPoint, _enemyPrefab.transform.rotation);
        dummy.HealthIndicator.Billboard.SetCamera(_cameraSystem.Camera);
        _healthSystem.AddCharacter(dummy.Health);
        dummy.Health.OnDeath += () => DummyDeathHandler(dummy);
    }

    private void DummyDeathHandler(Dummy dummy)
    {
        _healthSystem.RemoveCharacter(dummy.Health);
        StartCoroutine(DelayedSpawn());
    }

    private IEnumerator DelayedSpawn()
    {
        yield return _spawnInstruction;
        Spawn();
    }
}
