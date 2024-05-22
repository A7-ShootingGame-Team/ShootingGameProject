using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public Sprite[] characterImages;
    public Image selectedcharacter;
    

    public void ChoiceCharacter(int num)
    {
        selectedcharacter.sprite = characterImages[num];
        DataManager.instance.characterNum = num;

        if (num == 0)
        {
            DataManager.instance.damage = 1;
            DataManager.instance.bulletSpawnDelay = 0.25f;
        }
        else if (num == 1)
        {
            DataManager.instance.damage = 2;
            DataManager.instance.bulletSpawnDelay = 0.5f;
        }
    }
   
}
