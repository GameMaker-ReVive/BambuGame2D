using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed; // �ӵ�
    public float health; // ü��
    public float maxHealth; // �ִ� ü��
    public RuntimeAnimatorController[] animCon; // AnimatorController(��������Ʈ) ������ ���� ����
    public Rigidbody2D target; // ��ǥ

    bool isLive; // ��������

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
        // ������Ʈ Ȱ��ȭ ��, �ʱ� ������ ����
        isLive = true;
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (!isLive)
            return;

        // ��ġ ���� = Ÿ�� ��ġ - ���� ��ġ
        Vector2 dirVec = target.position - rigid.position; // ����
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;  // ������ ������ ��ġ�� ��
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // ���� �ӵ��� MovePosition �̵��� ������ ���� �ʵ��� �ӵ� ����
    }

    void LateUpdate()
    {
        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    // �ʱ� ������ �����ϴ� �Լ�
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
