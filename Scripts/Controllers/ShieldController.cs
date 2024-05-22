using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public float destroyTime = 5f;

    private void Start()
    {
        Destroy(gameObject, destroyTime); // 일정 시간 후 방패 제거
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy0") || collision.CompareTag("Enemy1") || collision.CompareTag("Enemy2"))
        {
            collision.gameObject.SetActive(false); // 적 제거
        }
        else if (collision.gameObject.CompareTag("EnemyBullet0") ||
                 collision.gameObject.CompareTag("EnemyBullet1") ||
                 collision.gameObject.CompareTag("EnemyBullet2"))
        {
            Destroy(collision.gameObject); // 총알 제거
        }
        else if (collision.CompareTag("Border"))
        {
            Destroy(gameObject); // 벽에 충돌 시 방패 제거
        }
    }
}
