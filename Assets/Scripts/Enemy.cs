using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        // ������Ʈ Ȱ��ȭ ��, �ʱ� ������ ����
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

        // ��ġ ���� = Ÿ�� ��ġ - ���� ��ġ
        Vector2 dirVec = target.position - rigid.position; // ����
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;  // ������ ������ ��ġ�� ��
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // ���� �ӵ��� MovePosition �̵��� ������ ���� �ʵ��� �ӵ� ����
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

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

    // �ִϸ��̼� �̺�Ʈ�� ���� ȣ��
    void Dead()
    {
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        yield return wait; //  ���� �ϳ��� ���� ������ ������

        Vector3 playerPos = GameManager.instance.player.transform.position;
        // �÷��̾� ������ �ݴ� ���� : ������ġ - �÷��̾� ��ġ
        Vector3 dirVec = transform.position - playerPos;

        // �˹� (�������� ���̹Ƿ� ForceMode2D.Impulse ���)
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
            // �ǰ� �ִϸ��̼�
            anim.SetTrigger("Hit");

            // ȿ����
            AudioManager.instance.PlayerSfx(AudioManager.Sfx.Hit);
        }
        else
        {
            // Die
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;

            // ���� ���̾� �ٿ�
            spriter.sortingOrder = 1;

            // �ִϸ��̼�
            anim.SetBool("Dead", true);

            // Die �Լ��� �ִϸ��̼��� �̺�Ʈ���� ����

            // ���ӸŴ��� ���� �߰�
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            // ȿ����
            if(GameManager.instance.isLive)
            {
                AudioManager.instance.PlayerSfx(AudioManager.Sfx.Dead);
            }
        }
    }
}
