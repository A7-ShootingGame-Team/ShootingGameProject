using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject[] lifeObjects;
    public GameObject endPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI nowScoreText;
    public TextMeshProUGUI bestScoreText;

    private int life;

    private void Awake()
    {
        Instance = this;

        life = lifeObjects.Length;
    }

    public void DecreaseLife()
    {
        if (GameManager.Instance.isGameOver) return;

        if (life > 0)
            life--;

        lifeObjects[life].SetActive(false);
        
        if (life == 0)
        {
            // 게임종료 
            Invoke("GameOver", 0.5f);
        }

    }
    void GameOver()
    {
        GameManager.Instance.GameOver();
    }
    public int getCurrentLife()
    {
        return life;
    }
    
}
