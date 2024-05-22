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
            // �ε��� ������Ʈ ü�� ���� (�÷��̾�)
            
            // �� ������Ʈ ���� (�߻�ü)
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Border")
        // �����ڸ� ����� �ı�
        {
            Destroy(gameObject);
        }
    }

   
}
