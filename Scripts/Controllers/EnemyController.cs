using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public int health;
    private int InitHealth;
    public int score;
    public float bulletSpawnDelay = 0.3f;
    public GameObject rewardPrefab;

    private Rigidbody2D rb;
    public Transform bulletSpawnPosition;
    public float currentDir;

    public AudioClip audioClip;
    private bool isAlive = false;

    private AnimationController animationController;
    private bool isBeingDestroyed = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletSpawnPosition = transform.Find("BulletSpawnPosition");
        animationController = GetComponent<AnimationController>();

        InitHealth = health; 

    }
    private void OnEnable()
    {
        // 체력 초기화
        health = InitHealth;

        // 스프라이트 및 애니메이션 상태 초기화
        animationController.ResetAnimation();
        isBeingDestroyed = false;
        isAlive = true;

    }

    private void OnHit()
    {
        health -= GameManager.Instance.damage;

        if (!isBeingDestroyed)
        {
            if (health <= 0)
            {
                isBeingDestroyed = true;
                if (isBeingDestroyed)
                    animationController.TriggerDestroy("Destroy", OnDestroyComplete);
            }
            if (isAlive) AudioManager.Instance.SFXPlay("explosion", audioClip, 0.2f);
            isAlive = false;

        }
    }

    private void OnDestroyComplete()
    {
        GameManager.Instance.AddScore(score);
        SpawnReward();
        gameObject.SetActive(false);
        gameObject.transform.rotation = Quaternion.identity;
    }

    private void SpawnReward()
    {
        if (rewardPrefab != null)
        {
            Instantiate(rewardPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Reward prefab is not assigned.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            animationController.setAnimTrigger("Hit");
            OnHit();
        }
        else if (collision.gameObject.tag == "Enemy Border")
        {
            gameObject.SetActive(false);
            gameObject.transform.rotation = Quaternion.identity;
        }
    }

    public void SetPattern(int ranPoint)
    {
        float rotateDegree = 45f;

        if (ranPoint == 5 || ranPoint == 6) // Right
        {
            transform.Rotate(Vector3.forward * rotateDegree);
            rb.velocity = new Vector2(speed, -1);
        }
        else if (ranPoint == 7 || ranPoint == 8) // Left 
        {
            transform.Rotate(Vector3.back * rotateDegree);
            rb.velocity = new Vector2(-speed, -1);
        }
        else
        {
            rb.velocity = Vector2.down * speed;
        }
        currentDir = transform.rotation.eulerAngles.z;
    }
}