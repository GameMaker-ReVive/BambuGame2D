using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed; // 속도
    public float health; // 체력
    public float maxHealth; // 최대 체력
    public RuntimeAnimatorController[] animCon; // AnimatorController(스프라이트) 변경을 위한 변수
    public Rigidbody2D target; // 목표

    bool isLive; // 생존여부

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        // 오브젝트 활성화 시, 초기 설정을 적용
        isLive = true;
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (!isLive)
            return;

        // 위치 차이 = 타겟 위치 - 나의 위치
        Vector2 dirVec = target.position - rigid.position; // 방향
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;  // 다음에 가야할 위치의 양
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // 물리 속도가 MovePosition 이동에 영향을 주지 않도록 속도 제거
    }

    void LateUpdate()
    {
        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    // 초기 설정을 적용하는 함수
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        health -= collision.GetComponent<Bullet>().damage;

        if(health > 0)
        {
            // Live, Hit Action
        }
        else
        {
            // Die
            Dead();
        }
    }
}
