using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed; // �ӵ�
    public Rigidbody2D target; // ��ǥ

    bool isLive; // ��������

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();

        isLive = true;
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
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

}
