using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private string[] enemyObjects;
    private string[] bullets;

    public Transform[] spawnPoints;
    public Transform[] bulletSpawnPoints;

    public float maxSpawnDelay;
    public float curSpawnDelay;

    public float TimeAfterPlayerBulletSpawn = 0f;

    public float bulletSpawnDelay = 0.1f; //플레이어 총알 딜레이 
    public int damage = 1;

    public ObjectPool pool;

    public int score;

    private int maxEnemyCount = 30;
    private int currentEnemyCount = 0;
    [SerializeField] private GameObject textBossWarning;
    [SerializeField] private GameObject boss;
    private bool isBossSpawned = false;

    int randomEnemy;
    EnemyController enemyController;

    public bool isGameOver = false;

    private Vector3 offSet = Vector3.right * 0.2f;

    private bool isSpawnboss = false;

    private AudioSource audioSource; 

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }



    // Start is called before the first frame update
    void Start()
    {
        bulletSpawnDelay = DataManager.instance.bulletSpawnDelay;
        damage = DataManager.instance.damage;

        enemyObjects = new string[] { "Enemy0", "Enemy1", "Enemy2" };
        bullets = new string[] { "PlayerBullet", "EnemyBullet0", "EnemyBullet1", "EnemyBullet2" };
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnemyCount == maxEnemyCount && !isBossSpawned)
        {
            StartCoroutine("SpawnBoss");
            isBossSpawned = true;
            return;
        }

        curSpawnDelay += Time.deltaTime;
        TimeAfterPlayerBulletSpawn += Time.deltaTime;

        if (!isGameOver)
        {
            if (curSpawnDelay > maxSpawnDelay)
            {
                if (!isBossSpawned) SpawnEnemy();
                maxSpawnDelay = Random.Range(1f, 2f);
                curSpawnDelay = 0f;
                currentEnemyCount++;

            }
            if (TimeAfterPlayerBulletSpawn > bulletSpawnDelay)
            {
                audioSource.PlayOneShot(audioSource.clip, 0.3f);
                SpawnBullet();
                TimeAfterPlayerBulletSpawn = 0f;
            }
        }


    }

    public void SettingPlayer(Transform _transform)
    {
        isGameOver = false;
        bulletSpawnPoints[0] = _transform;
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GameOver()
    {

        SetBestScore();

        isGameOver = true;

        StopAllCoroutines();

        UIManager.Instance.endPanel.SetActive(true);

    }

    private void SetBestScore()
    {
        UIManager.Instance.nowScoreText.text = score.ToString();

        if (PlayerPrefs.GetInt("bestScore") < score)
        {
            PlayerPrefs.SetInt("bestScore", score);
        }

        UIManager.Instance.bestScoreText.text = PlayerPrefs.GetInt("bestScore").ToString();

    }

    private IEnumerator SpawnBoss()
    {
        textBossWarning.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        textBossWarning.SetActive(false);

        boss.SetActive(true);
        boss.GetComponent<BossController>().ChangeState(BossState.MoveToAppearPoint);
    }

    private void SpawnEnemy()
    {
        randomEnemy = Random.Range(0, 3);
        int randomPoint = Random.Range(0, 9);

        GameObject enemy = pool.SpawnFromPool(enemyObjects[randomEnemy]);
        enemy.transform.position = spawnPoints[randomPoint].position;

        enemyController = enemy.GetComponent<EnemyController>();
        enemyController.SetPattern(randomPoint);

        StartCoroutine(SpawnEnemyBullet(enemyController, randomEnemy));
    }

    public void AddScore(int enemyScore)
    {
        score += enemyScore;
        UIManager.Instance.scoreText.text = score.ToString();
    }

    void SpawnBullet()
    {
        GameObject bullet = pool.SpawnFromPool(bullets[0]);
        bullet.transform.position = bulletSpawnPoints[0].position + offSet;

        Bullet bulletDirection = bullet.GetComponent<Bullet>();
        bulletDirection.SetDirection(false, 0);
    }

    IEnumerator SpawnEnemyBullet(EnemyController enemy, int enemyIndex)
    {
        while (enemy.gameObject.activeSelf)
        {
            yield return new WaitForSeconds(enemy.bulletSpawnDelay);
            // 적이 아직 활성화된 상태인지 확인(WaitForSeconds 동안 비활성화 될 수 있으므로)
            if (enemy.gameObject.activeSelf)
            {
                GameObject enemyBullet = pool.SpawnFromPool(bullets[enemyIndex + 1]);
                enemyBullet.transform.position = enemy.bulletSpawnPosition.position;

                Bullet bulletDirection = enemyBullet.GetComponent<Bullet>();
                bulletDirection.SetDirection(true, enemy.currentDir);
            }
        }
    }

}
