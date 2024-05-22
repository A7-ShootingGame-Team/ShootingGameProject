using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum AttackType { CircleFire = 0, SingleFireToCenterPosition, SpiralFire, SpreadFire, CombinedWaveSpreadFire, OrbitFire, CentralToCircularPattern }

public class BossWeapon : MonoBehaviour
{
    [SerializeField] public GameObject projectilePrefab;   // ������ �� �����Ǵ� �߻�ü ������

    public void StartFiring(AttackType attackType)
    {
        // attackType �������� �̸��� ���� �ڷ�ƾ�� ����
        StartCoroutine(attackType.ToString());
    }

    public void StopFiring(AttackType attackType)
    {
        // attackType �������� �̸��� ���� �ڷ�ƾ�� ����
        StopCoroutine(attackType.ToString());
    }

    private IEnumerator CircleFire()
    {
        float attackRate = 0.8f;            // ���� �ֱ�
        int count = 30;         // �߻�ü ���� ����
        float intervalAngle = 360 / count;  // �߻�ü ������ ����
        float weightAngle = 0;          // ���ߵǴ� ���� (�׻� ���� ��ġ�� �߻����� �ʵ��� ����)

        // �� ���·� ����ϴ� �߻�ü ���� (count ������ŭ)
        while (true)
        {
            for (int i = 0; i < count; ++i)
            {
                // �߻�ü ����
                GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                // �߻�ü �̵� ���� (����)
                float angle = weightAngle + intervalAngle * i;
                // �߻�ü �̵� ���� (����)
                float x = Mathf.Cos(angle * Mathf.PI / 180.0f); // Cos(����), ���� ������ ���� ǥ���� ���� PI / 180�� ����
                float y = Mathf.Sin(angle * Mathf.PI / 180.0f); // Sin(����), ���� ������ ���� ǥ���� ���� PI / 180�� ����
                                                                // �߻�ü �̵� ���� ����
                clone.GetComponent<BossMovement>().MoveTo(new Vector2(x, y));
            }

            // �߻�ü�� �����Ǵ� ���� ���� ������ ���� ����
            weightAngle += 1;

            // attackRate �ð���ŭ ���
            yield return new WaitForSeconds(attackRate);
        }
    }

    private IEnumerator SingleFireToCenterPosition()
    {
        Vector3 targetPosition = Vector3.zero;  // ��ǥ ��ġ (�߾�)
        float attackRate = 8f;

        while (true)
        {
            // �߻�ü ����
            GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            // �߻�ü �̵� ����
            Vector3 direction = (targetPosition - clone.transform.position).normalized;
            // �߻�ü �̵� ���� ����
            clone.GetComponent<BossMovement>().MoveTo(direction);

            // attackRate �ð���ŭ ���
            yield return new WaitForSeconds(attackRate);
        }
    }

    private IEnumerator SpiralFire()
    {
        float attackRate = 0.2f; // ���� �ֱ�
        float spiralSpeed = 4f; // ������ �ӵ�
        float angle = -90f; // �ʱ� ����

        while (true)
        {
            // �߻�ü ����
            GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            // �߻�ü �̵� ���� (����)
            float x = Mathf.Cos(angle * Mathf.PI / 45.0f);
            float y = Mathf.Sin(angle * Mathf.PI / 45.0f);
            // �߻�ü �̵� ���� ����
            clone.GetComponent<BossMovement>().MoveTo(new Vector2(x, y));

            // ������ ���������� ����
            angle += spiralSpeed;

            // attackRate �ð���ŭ ���
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
        int spreadCount = 10; // �߻�ü ���� ����
        float spreadAngle = 180f; // ������ ����
        float waveFrequency = 10f; // �ĵ� ��
        float waveAmplitude = 10f; // �ĵ� ����

        while (true)
        {
            for (int i = -spreadCount / 2; i <= spreadCount / 2; i++)
            {
                // �߻�ü ����
                GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                // ������ �ĵ� ���·� ����
                float adjustedAngle = -90f + i * (spreadAngle / spreadCount) + waveFrequency * Mathf.Sin(Time.time * waveAmplitude); // -90���� ���� �Ʒ� �������� ����
                float x = Mathf.Cos(adjustedAngle * Mathf.PI / 180.0f);
                float y = Mathf.Sin(adjustedAngle * Mathf.PI / 180.0f);
                Vector2 direction = new Vector2(x, y).normalized;
                // �߻�ü �̵� ���� ����
                clone.GetComponent<BossMovement>().MoveTo(direction);
            }

            yield return new WaitForSeconds(attackRate);
        }
    }


    private IEnumerator OrbitFire()
    {
        float attackRate = 0.05f; // �߻�ü ���� �ֱ�
        int projectileCount = 50; // �߻�ü ����
        float orbitRadius = 2f; // �˵� �ݰ�
        float orbitSpeed = 5000f; // �˵� ȸ�� �ӵ�
        float spreadAngle = 480f; // ������ ����

        // �߻�ü �迭
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
                // �Ѿ��� �ı��Ǿ����� Ȯ��
                if (projectiles[i] == null)
                {
                    // �Ѿ��� �ı��Ǿ����� �ٽ� ����
                    float angle2 = i * angleStep;
                    Vector3 position2 = new Vector3(
                        transform.position.x + Mathf.Cos(angle2 * Mathf.Deg2Rad) * orbitRadius,
                        transform.position.y + Mathf.Sin(angle2 * Mathf.Deg2Rad) * orbitRadius,
                        transform.position.z
                    );

                    GameObject clone = Instantiate(projectilePrefab, position2, Quaternion.identity);
                    projectiles[i] = clone;
                    continue; // �ٽ� ���������Ƿ� ���� �Ѿ˷� �Ѿ
                }

                // ������ �������� ����
                float angle = i * angleStep + Time.time * orbitSpeed;
                float radius = orbitRadius * Mathf.Lerp(1f, 4f, Mathf.PingPong(Time.time, 1f)); // �ݺ������� �������� �� ū ������ ��ȭ���� ������ ȿ���� ��
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
        float attackRate = 0.05f; // �߻�ü ���� �ֱ�
        int projectileCount = 50; // �߻�ü ����
        float orbitRadius = 2f; // �˵� �ݰ�
        float centralFireDuration = 0f;
        float circularSpreadDuration = 0f;
        // �߾����� �̵���ų �Ѿ˵��� ����
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            clone.GetComponent<BossMovement>().MoveTo(Vector3.zero);
            yield return new WaitForSeconds(attackRate);
        }

        yield return new WaitForSeconds(centralFireDuration);

        // �߾ӿ� �ִ� �Ѿ��� �����鼭 �������� ������ ȿ��
        float startTime = Time.time;
        while (Time.time - startTime < circularSpreadDuration)
        {
            // �߾ӿ� �ִ� �Ѿ� ����
            for (int i = 0; i < projectileCount; i++)
            {
                float angle = i * (360f / projectileCount);
                Vector3 position = transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius, Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius, 0f);
                GameObject clone = Instantiate(projectilePrefab, position, Quaternion.identity);
                // �߾ӿ��� �������� ����
                clone.GetComponent<BossMovement>().MoveTo((position - transform.position).normalized);
            }

            yield return null;
        }
    }
}






/*
 *	: ���� ĳ������ ���� ����. �߻�ü ����
 *	
 * Functions
 *	: StartFiring() - ���� ����
 *  : StopFiring() - ���� ����
 *  : CircleFire() - ���� ���
 *  : SingleFireToCenterPosition() - �߾���ġ�� ������ �߻�ü �ϳ� ����
 *	: OrbitFire() - ���ۺ��� ���� �Ѿ�
 *	: CombinedWaveSpreadFire() - ���̺� �Ѿ�
 *	
 */