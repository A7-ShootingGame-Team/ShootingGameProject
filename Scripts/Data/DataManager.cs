using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public int characterNum;
    public float bulletSpawnDelay;
    public int damage;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
