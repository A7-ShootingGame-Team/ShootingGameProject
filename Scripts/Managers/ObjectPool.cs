using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools = new List<Pool>();
    public Dictionary<string, Queue<GameObject>> PoolDicionary;

    private void Awake()
    {
        GameManager.Instance.pool = this;

        PoolDicionary = new Dictionary<string, Queue<GameObject>>();
        
        foreach (var pool in pools)
        {
            Queue<GameObject> queue = new Queue<GameObject>();  

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

            PoolDicionary.Add(pool.tag, queue);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {

        if (!PoolDicionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject obj = PoolDicionary[tag].Dequeue();
        PoolDicionary[(tag)].Enqueue(obj);

        obj.SetActive(true);

        return obj;
    }

}
