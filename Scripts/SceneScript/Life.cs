using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{

    private GameObject[] lifes;
    private int life;

    private void Awake()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        
        for(int i = 0; i < children.Length; i++)
        {
            lifes[i] = children[i].gameObject;
        }

        life = lifes.Length;
    }

    public void DecreaseLife()
    {
        life--;

        lifes[life].SetActive(false);

    }

}
