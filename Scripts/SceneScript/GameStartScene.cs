using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartScene : MonoBehaviour
{
    public void GameStartBtn()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void MainBtn()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void CharacterBtn()
    {
        SceneManager.LoadScene("CharacterScene");
    }
   
}
