using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public float destroyTime = 5f;

    private void Start()
    {
        Destroy(gameObject, destroyTime); // ���� �ð� �� ���� ����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy0") || collision.CompareTag("Enemy1") || collision.CompareTag("Enemy2"))
        {
            collision.gameObject.SetActive(false); // �� ����
        }
        else if (collision.gameObject.CompareTag("EnemyBullet0") ||
                 collision.gameObject.CompareTag("EnemyBullet1") ||
                 collision.gameObject.CompareTag("EnemyBullet2"))
        {
            Destroy(collision.gameObject); // �Ѿ� ����
        }
        else if (collision.CompareTag("Border"))
        {
            Destroy(gameObject); // ���� �浹 �� ���� ����
        }
    }
}
