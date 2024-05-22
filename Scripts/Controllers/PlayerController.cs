using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private AnimationController animationController;
    internal bool isShielded;
    private void Awake()
    {
        GameManager.Instance.SettingPlayer(transform);
        animationController = GetComponent<AnimationController>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet0" || collision.gameObject.tag == "EnemyBullet1" || collision.gameObject.tag == "EnemyBullet2")
        {
            animationController.setAnimTrigger("Hit");
            UIManager.Instance.DecreaseLife();

            //현재 라이프가 모두 소진되었을 때, 
            if (UIManager.Instance.getCurrentLife() <= 0)
            {
                PlayerDestroyAnim();
            }
        }
    }

    private void PlayerDestroyAnim()
    {
        if (DataManager.instance.characterNum == 0)
        {
            animationController.setAnimTrigger("Player1Destroy");
        }
        else if (DataManager.instance.characterNum == 1)
        {
            animationController.setAnimTrigger("Player2Destroy");
        }
    }
    
}
