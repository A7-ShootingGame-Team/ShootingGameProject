using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetSelectedSprite : MonoBehaviour
{
    public GameObject[] players;
    int idx = 0;

    private void Start()
    {
        if(DataManager.instance != null)
            idx = DataManager.instance.characterNum;

        players[idx].SetActive(true);
    }
}
