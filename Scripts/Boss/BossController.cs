using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BossState { MoveToAppearPoint = 0, Phase01, Phase02, Phase03, Phase04 }

public class BossController : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float bossAppearPoint = 2.5f;
    private BossState bossState = BossState.MoveToAppearPoint;
    private BossMovement movement2D;
    private BossWeapon bossWeapon;
    private EnemyController enemyController;

    private void Awake()
    {
        movement2D = GetComponent<BossMovement>();
        bossWeapon = GetComponent<BossWeapon>();
        enemyController = GetComponent<EnemyController>();

        if (enemyController == null)
        {
            Debug.LogError("EnemyController is missing on the Boss object.");
        }
    }

    public void ChangeState(BossState newState)
    {
        // 현재 코루틴이 실행중이면 중지
        if (IsInvoking(bossState.ToString()))
        {
            StopCoroutine(bossState.ToString());
        }

        // 상태 변경
        bossState = newState;

        // 새로운 상태 재생
        StartCoroutine(bossState.ToString());
    }

    private IEnumerator MoveToAppearPoint()
    {
        movement2D.MoveTo(Vector3.down);

        while (true)
        {
            if (transform.position.y <= bossAppearPoint)
            {
                movement2D.MoveTo(Vector3.zero);
                ChangeState(BossState.Phase01);
                yield break; // 루프 종료
            }
            yield return null;
        }
    }

    private IEnumerator Phase01()
    {
        
        bossWeapon.StartFiring(AttackType.CombinedWaveSpreadFire);
        while (true)
        {
            if (enemyController.health <= 100 * 0.7f)
            {
                bossWeapon.StopFiring(AttackType.CombinedWaveSpreadFire);
                ChangeState(BossState.Phase02);
                yield break; // 루프 종료
            }
            yield return null;
        }
    }

    private IEnumerator Phase02()
    {
        bossWeapon.StartFiring(AttackType.SpreadFire);
        
        Vector3 direction = Vector3.right;
        movement2D.MoveTo(direction);

        while (true)
        {
            if (transform.position.x <= -2.5f || transform.position.x >= 2.5f)
            {
                direction *= -1;
                movement2D.MoveTo(direction);
            }

            if (enemyController.health <= 100 * 0.3f)
            {
                bossWeapon.StopFiring(AttackType.SpreadFire);
                ChangeState(BossState.Phase03);
                yield break; // 루프 종료
            }

            yield return null;
        }
    }

    private IEnumerator Phase03()
    {
        // 원 방사 형태의 공격 시작
        bossWeapon.StartFiring(AttackType.CircleFire);
        // 플레이어 위치를 기준으로 단일 발사체 공격 시작
        bossWeapon.StartFiring(AttackType.SingleFireToCenterPosition);
        
        // 처음 이동 방향을 오른쪽으로 설정
        Vector3 direction = Vector3.right;
        movement2D.MoveTo(direction);

        while (true)
        {
            // 좌-우 이동 중 양쪽 끝에 도달하게 되면 방향을 반대로 설정
            if (transform.position.x <= -2.8f || transform.position.x >= 2.8f)
            {
                direction *= -1;
                movement2D.MoveTo(direction);
            }

            yield return null;
        }
    }



}
