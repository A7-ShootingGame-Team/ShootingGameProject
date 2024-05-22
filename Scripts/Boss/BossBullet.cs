using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 부딛힌 오브젝트 체력 감소 (플레이어)
            
            // 내 오브젝트 삭제 (발사체)
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Border")
        // 가장자리 벗어나면 파괴
        {
            Destroy(gameObject);
        }
    }

   
}
