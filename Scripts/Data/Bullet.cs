using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    [SerializeField] bool isEnemy = false; // 적군의 총알 여부
    [SerializeField] bool isTargetingBullet = false; // 유도 미사일 여부
    private bool enterTarget = false; //타겟 위치에 도착햇는지 여부

    private Vector3 targetPosition; // 목표 위치

    public float speed = 8f; // 총알 속도
    private Rigidbody2D bulletRigidbody;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();

        if (isTargetingBullet)
        {
            // 플레이어 오브젝트를 찾고 목표 위치를 설정
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                targetPosition = player.transform.position;
            }
        }
        SetDirection(isEnemy, 0);
    }

    private void OnEnable()
    {
        if (isTargetingBullet)
        {
            // 플레이어 오브젝트를 찾고 목표 위치를 설정
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                targetPosition = player.transform.position;
            }
        }
        enterTarget = false;
        SetDirection(isEnemy, 0);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 가장자리에 닿으면 총알 비활성화
        if (collision.gameObject.tag == "Border")
        {
            gameObject.SetActive(false);
        }
        // 적의 총알이 플레이어와 닿았을 때 비활성화
        if (isEnemy && collision.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
        }
        // 플레이어의 총알이 적과 닿았을 때 비활성화
        if (!isEnemy)
        {
            if (collision.gameObject.tag.Contains("Enemy"))
            {
                if(!collision.gameObject.tag.Contains("Bullet"))
                    gameObject.SetActive(false);
            }
        }
    }
    private void FixedUpdate()
    {
        if (isTargetingBullet && targetPosition != Vector3.zero)
        {
            //타겟 위치에 도착하지 않았을 때에만 유도탄 방향을 갱신
            if (!enterTarget)
            {
                // 타겟(플레이어)을 향한 방향 계산
                Vector2 direction = ((Vector2)targetPosition - bulletRigidbody.position).normalized;
                bulletRigidbody.velocity = direction * speed;

                // 총알이 목표 위치에 도달했는지 확인(거리가 0.1 이하일때로 가정)
                if (Vector3.Distance(bulletRigidbody.position, targetPosition) <= 0.5f)
                {
                    enterTarget = true;
                }
                else
                {
                    // 총알을 방향에 맞춰 회전
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                    bulletRigidbody.rotation = angle;
                }
            }
        }
    }
    
    public void SetDirection(bool isEnemy, float rotationAngle)
    {
        if (bulletRigidbody != null)
        {
            Vector3 direction;

            if (isEnemy)
            {
                // 적군의 총알은 회전된 방향의 아래로 날아감
                direction = Quaternion.Euler(0, 0, rotationAngle) * Vector3.down;

                // 스프라이트를 현재 rotationAngle만큼 Z축으로 회전
                transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
            }
            else
            {
                // 플레이어의 총알은 위로 날아감
                direction = Vector3.up;
            }

            bulletRigidbody.velocity = direction * speed;
            
        }
    }
}
