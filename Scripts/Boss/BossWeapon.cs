using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum AttackType { CircleFire = 0, SingleFireToCenterPosition, SpiralFire, SpreadFire, CombinedWaveSpreadFire, OrbitFire, CentralToCircularPattern }

public class BossWeapon : MonoBehaviour
{
    [SerializeField] public GameObject projectilePrefab;   // 공격할 때 생성되는 발사체 프리팹

    public void StartFiring(AttackType attackType)
    {
        // attackType 열거형의 이름과 같은 코루틴을 실행
        StartCoroutine(attackType.ToString());
    }

    public void StopFiring(AttackType attackType)
    {
        // attackType 열거형의 이름과 같은 코루틴을 중지
        StopCoroutine(attackType.ToString());
    }

    private IEnumerator CircleFire()
    {
        float attackRate = 0.8f;            // 공격 주기
        int count = 30;         // 발사체 생성 개수
        float intervalAngle = 360 / count;  // 발사체 사이의 각도
        float weightAngle = 0;          // 가중되는 각도 (항상 같은 위치로 발사하지 않도록 설정)

        // 원 형태로 방사하는 발사체 생성 (count 개수만큼)
        while (true)
        {
            for (int i = 0; i < count; ++i)
            {
                // 발사체 생성
                GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                // 발사체 이동 방향 (각도)
                float angle = weightAngle + intervalAngle * i;
                // 발사체 이동 방향 (벡터)
                float x = Mathf.Cos(angle * Mathf.PI / 180.0f); // Cos(각도), 라디안 단위의 각도 표현을 위해 PI / 180을 곱함
                float y = Mathf.Sin(angle * Mathf.PI / 180.0f); // Sin(각도), 라디안 단위의 각도 표현을 위해 PI / 180을 곱함
                                                                // 발사체 이동 방향 설정
                clone.GetComponent<BossMovement>().MoveTo(new Vector2(x, y));
            }

            // 발사체가 생성되는 시작 각도 설정을 위한 변수
            weightAngle += 1;

            // attackRate 시간만큼 대기
            yield return new WaitForSeconds(attackRate);
        }
    }

    private IEnumerator SingleFireToCenterPosition()
    {
        Vector3 targetPosition = Vector3.zero;  // 목표 위치 (중앙)
        float attackRate = 8f;

        while (true)
        {
            // 발사체 생성
            GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            // 발사체 이동 방향
            Vector3 direction = (targetPosition - clone.transform.position).normalized;
            // 발사체 이동 방향 설정
            clone.GetComponent<BossMovement>().MoveTo(direction);

            // attackRate 시간만큼 대기
            yield return new WaitForSeconds(attackRate);
        }
    }

    private IEnumerator SpiralFire()
    {
        float attackRate = 0.2f; // 공격 주기
        float spiralSpeed = 4f; // 나선형 속도
        float angle = -90f; // 초기 각도

        while (true)
        {
            // 발사체 생성
            GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            // 발사체 이동 방향 (각도)
            float x = Mathf.Cos(angle * Mathf.PI / 45.0f);
            float y = Mathf.Sin(angle * Mathf.PI / 45.0f);
            // 발사체 이동 방향 설정
            clone.GetComponent<BossMovement>().MoveTo(new Vector2(x, y));

            // 각도를 나선형으로 증가
            angle += spiralSpeed;

            // attackRate 시간만큼 대기
            yield return new WaitForSeconds(attackRate);
        }
    }


    private IEnumerator SpreadFire()
    {
        float attackRate = 0.1f;
        int spreadCount = 20;
        float spreadAngle = 45f;

        while (true)
        {
            for (int i = -spreadCount / 2; i <= spreadCount / 2; i++)
            {
                GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                float angle = i * (spreadAngle / spreadCount);
                float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
                float y = Mathf.Sin(angle * Mathf.PI / 180.0f);
                Vector3 direction = new Vector3(x, y, 0);
                clone.GetComponent<BossMovement>().MoveTo((Vector3.zero - clone.transform.position).normalized);
            }

            yield return new WaitForSeconds(attackRate);
        }
    }
    private IEnumerator CombinedWaveSpreadFire()
    {
        float attackRate = 0.2f;
        int spreadCount = 10; // 발사체 생성 개수
        float spreadAngle = 180f; // 퍼지는 각도
        float waveFrequency = 10f; // 파동 빈도
        float waveAmplitude = 10f; // 파동 진폭

        while (true)
        {
            for (int i = -spreadCount / 2; i <= spreadCount / 2; i++)
            {
                // 발사체 생성
                GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                // 각도를 파동 형태로 변경
                float adjustedAngle = -90f + i * (spreadAngle / spreadCount) + waveFrequency * Mathf.Sin(Time.time * waveAmplitude); // -90도를 더해 아래 방향으로 설정
                float x = Mathf.Cos(adjustedAngle * Mathf.PI / 180.0f);
                float y = Mathf.Sin(adjustedAngle * Mathf.PI / 180.0f);
                Vector2 direction = new Vector2(x, y).normalized;
                // 발사체 이동 방향 설정
                clone.GetComponent<BossMovement>().MoveTo(direction);
            }

            yield return new WaitForSeconds(attackRate);
        }
    }


    private IEnumerator OrbitFire()
    {
        float attackRate = 0.05f; // 발사체 생성 주기
        int projectileCount = 50; // 발사체 개수
        float orbitRadius = 2f; // 궤도 반경
        float orbitSpeed = 5000f; // 궤도 회전 속도
        float spreadAngle = 480f; // 퍼지는 각도

        // 발사체 배열
        GameObject[] projectiles = new GameObject[projectileCount];
        float angleStep = spreadAngle / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * angleStep;
            Vector3 position = new Vector3(
                transform.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius,
                transform.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius,
                transform.position.z
            );

            GameObject clone = Instantiate(projectilePrefab, position, Quaternion.identity);
            projectiles[i] = clone;
        }

        while (true)
        {
            for (int i = 0; i < projectileCount; i++)
            {
                // 총알이 파괴되었는지 확인
                if (projectiles[i] == null)
                {
                    // 총알이 파괴되었으면 다시 생성
                    float angle2 = i * angleStep;
                    Vector3 position2 = new Vector3(
                        transform.position.x + Mathf.Cos(angle2 * Mathf.Deg2Rad) * orbitRadius,
                        transform.position.y + Mathf.Sin(angle2 * Mathf.Deg2Rad) * orbitRadius,
                        transform.position.z
                    );

                    GameObject clone = Instantiate(projectilePrefab, position2, Quaternion.identity);
                    projectiles[i] = clone;
                    continue; // 다시 생성했으므로 다음 총알로 넘어감
                }

                // 퍼지는 움직임을 적용
                float angle = i * angleStep + Time.time * orbitSpeed;
                float radius = orbitRadius * Mathf.Lerp(1f, 4f, Mathf.PingPong(Time.time, 1f)); // 반복적으로 반지름을 더 큰 값으로 변화시켜 퍼지는 효과를 줌
                Vector3 position = new Vector3(
                    transform.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                    transform.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                    transform.position.z
                );

                projectiles[i].transform.position = position;
            }

            yield return new WaitForSeconds(attackRate);
        }
    }
    private IEnumerator CentralToCircularPattern()
    {
        float attackRate = 0.05f; // 발사체 생성 주기
        int projectileCount = 50; // 발사체 개수
        float orbitRadius = 2f; // 궤도 반경
        float centralFireDuration = 0f;
        float circularSpreadDuration = 0f;
        // 중앙으로 이동시킬 총알들을 생성
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            clone.GetComponent<BossMovement>().MoveTo(Vector3.zero);
            yield return new WaitForSeconds(attackRate);
        }

        yield return new WaitForSeconds(centralFireDuration);

        // 중앙에 있는 총알이 터지면서 원형으로 퍼지는 효과
        float startTime = Time.time;
        while (Time.time - startTime < circularSpreadDuration)
        {
            // 중앙에 있는 총알 생성
            for (int i = 0; i < projectileCount; i++)
            {
                float angle = i * (360f / projectileCount);
                Vector3 position = transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius, Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius, 0f);
                GameObject clone = Instantiate(projectilePrefab, position, Quaternion.identity);
                // 중앙에서 퍼지도록 설정
                clone.GetComponent<BossMovement>().MoveTo((position - transform.position).normalized);
            }

            yield return null;
        }
    }
}






/*
 *	: 보스 캐릭터의 공격 관리. 발사체 생성
 *	
 * Functions
 *	: StartFiring() - 공격 시작
 *  : StopFiring() - 공격 중지
 *  : CircleFire() - 원형 방사
 *  : SingleFireToCenterPosition() - 중앙위치를 지나는 발사체 하나 생성
 *	: OrbitFire() - 빙글빙글 도는 총알
 *	: CombinedWaveSpreadFire() - 웨이브 총알
 *	
 */