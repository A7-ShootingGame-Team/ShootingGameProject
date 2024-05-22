using UnityEngine;

public class RewardSpawn : MonoBehaviour
{
    private PlayerSkillManager playerSkillManager; // 플레이어의 스킬 매니저를 참조하기 위한 변수

    private void Awake()
    {
        playerSkillManager = FindObjectOfType<PlayerSkillManager>(); // PlayerSkillManager 스크립트 참조
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 보상 획득 처리 (예: 플레이어의 스킬 포인트 증가)
            if (playerSkillManager != null)
            {
                playerSkillManager.AddSkillPoints(1); // 보상 포인트 증가
            }

            // 보상 오브젝트 비활성화
            gameObject.SetActive(false);
        }
    }
}