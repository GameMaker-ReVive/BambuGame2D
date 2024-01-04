using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    Collider2D coll;
    SpriteRenderer spriter;
    Animator anim;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();

        // 오브젝트 활성화 시, 초기 설정을 적용
        isLive = true;
        health = maxHealth;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);

    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        // 위치 차이 = 타겟 위치 - 나의 위치
        Vector2 dirVec = target.position - rigid.position; // 방향
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;  // 다음에 가야할 위치의 양
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // 물리 속도가 MovePosition 이동에 영향을 주지 않도록 속도 제거
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

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

    // 애니메이션 이벤트를 통해 호출
    void Dead()
    {
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        yield return wait; //  다음 하나의 물리 프레임 딜레이

        Vector3 playerPos = GameManager.instance.player.transform.position;
        // 플레이어 기준의 반대 방향 : 현재위치 - 플레이어 위치
        Vector3 dirVec = transform.position - playerPos;

        // 넉백 (순간적인 힘이므로 ForceMode2D.Impulse 사용)
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if(health > 0)
        {
            // 피격 애니메이션
            anim.SetTrigger("Hit");

            // 효과음
            AudioManager.instance.PlayerSfx(AudioManager.Sfx.Hit);
        }
        else
        {
            // Die
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;

            // 오더 레이어 다운
            spriter.sortingOrder = 1;

            // 애니메이션
            anim.SetBool("Dead", true);

            // Die 함수는 애니메이션의 이벤트에서 설정

            // 게임매니저 정보 추가
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            // 효과음
            if(GameManager.instance.isLive)
            {
                AudioManager.instance.PlayerSfx(AudioManager.Sfx.Dead);
            }
        }
    }
}
