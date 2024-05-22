using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillManager : MonoBehaviour
{
    public static PlayerSkillManager Instance;

    public int skillPoints;
    public Slider skillPointsSlider;
    public int pointsToUseSkill = 10;
    public int ShieldUseSkill = 5;


    public GameObject shieldPrefab; // 방패 프리팹을 여기서 참조
    public float shieldSpeed = 10f;

    private Transform playerTransform;

    private List<GameObject> enemiesList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;        
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        enemiesList = new List<GameObject>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found!");
        }

        UpdateSkillPointsUI();
    }

    public void AddSkillPoints(int points)
    {
        skillPoints += points;
        if (skillPoints > skillPointsSlider.maxValue)
        {
            skillPoints = (int)skillPointsSlider.maxValue;
        }
        UpdateSkillPointsUI();
    }

    private void UpdateSkillPointsUI()
    {
        skillPointsSlider.value = skillPoints;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && skillPoints >= pointsToUseSkill)
        {
            UseSkill();
        }

        if (Input.GetKeyDown(KeyCode.C) && skillPoints >= ShieldUseSkill)
        {
            LaunchShield();
            skillPoints -= ShieldUseSkill;
            UpdateSkillPointsUI();
        }
    }

    private void UseSkill()
    {
        DestroyAllEnemies();
        skillPoints -= pointsToUseSkill;
        UpdateSkillPointsUI();
    }

    private void DestroyAllEnemies()
    {
        GameObject[] enemies0 = GameObject.FindGameObjectsWithTag("Enemy0");
        GameObject[] enemies1 = GameObject.FindGameObjectsWithTag("Enemy1");
        GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("Enemy2");

        enemiesList.AddRange(enemies0);
        enemiesList.AddRange(enemies1);
        enemiesList.AddRange(enemies2);

        foreach (GameObject enemy in enemiesList)
        {
            AnimationController animationController = enemy.GetComponent<AnimationController>();
            if (animationController != null)
            {
                animationController.setAnimTrigger("Skill");

                //람다식: AnimationController에서 Action을 통해 등록할 함수는 매개변수가 없는 함수만 해당하므로,
                //람다식을 사용하여 매개변수가 없는 익명 메서드를 정의함
                animationController.TriggerSkillDestroy("Destroy", () => DestroyEnemy(enemy));
            }
        }
    }

    private void LaunchShield()
    {
        if (shieldPrefab != null && playerTransform != null)
        {
            GameObject shield = Instantiate(shieldPrefab, playerTransform.position, Quaternion.identity);
            Rigidbody2D rb = shield.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = playerTransform.up * shieldSpeed; // 플레이어의 오른쪽 방향으로 발사
            }
        }
        else
        {
            Debug.LogWarning("Shield Prefab or Player Transform not assigned!");
        }
    }
    private void DestroyEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
    }
}
