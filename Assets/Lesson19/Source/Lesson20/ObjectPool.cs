using System.Collections.Generic;
using MyLesson19.BehaviourTreeSystem;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private EnemyController prefab;
    public int initialPoolSize = 10;

    private Queue<EnemyController> pool = new Queue<EnemyController>();

    private void Start()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            EnemyController obj = Instantiate(prefab, transform);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public EnemyController GetFromPool(Vector3 position, Quaternion rotation)
    {
        if (pool.Count > 0)
        {
            EnemyController obj = pool.Dequeue();
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            //EnemyController newObj = Instantiate(prefab);
            //newObj.gameObject.SetActive(true);
            //return newObj;

            //EnemyController newObj = Instantiate(prefab);
            //newObj.gameObject.SetActive(false);
            //pool.Enqueue(newObj);
            //return GetFromPool(); // рекурсивно повертаємо з пулу

            EnemyController newObj = Instantiate(prefab, position, rotation);
            return newObj;
        }
    }

    public void ReturnToPool(EnemyController obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}