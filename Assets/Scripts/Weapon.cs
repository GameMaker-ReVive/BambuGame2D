using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id; // ���� ����
    public int prefabId; // ������ ID
    public float damage; // ������
    public int count; // ����
    public float speed; // �ӵ�

    float timer; // �Ѿ� �߻� Ÿ�̸�
    Player player;

    void Awake()
    {
        // GetComponentInParent : �θ��� ������Ʈ�� ������.
        player = GetComponentInParent<Player>();
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                // Vector3.forward -> (0, 0, 1)
                // Vector3.back -> (0, 0, -1)
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
    }

    // ������
    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
        {
            Batch();
        }
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = -150; // Vector3.forward �� ����ϱ⿡ ���������� �ð�������� ȸ��
                Batch();
                break;
            default:
                // speed ���� ����ӵ��� �ǹ� -> ���� ���� ���� �߻�
                speed = 0.3f;
                break;
        }
    }

    // ��ġ
    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            if (index < transform.childCount) // childCount : �ڽ� ������Ʈ ���� Ȯ��
            {
                // count �� ������ �ִ� bullet ���� �����ؼ� ���
                // if (index < transform.childCount) �� ������ ���� bullet �� �����ϰ� count ��ŭ �ٽ� ������.
                bullet = transform.GetChild(index);
            }
            else
            {
                // ������ bullet ���� count �� Ŭ ��, �׸�ŭ�� pool ���� �����ͼ� �� ����
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform; // ���Ӱ� ������� ������Ʈ�� �θ� �ڱ��ڽ����� ����
            }

            // �ʱ�ȭ
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // ȸ��
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);

            // ��ġ
            bullet.Translate(bullet.up * 1.5f, Space.World);

            // �� ����
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 �� ����� ������ �ǹ�
        }
    }

    // �Ѿ� �߻�
    void Fire()
    {
        // �νĵ� ���� �ִ� �� ���� �ľ�
        if(!player.scanner.nearestTarget)
            return;

        // ��ǥ ��ġ�� ���� ���ϱ�
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized; // normalized : ���� ������ ������ �����ϰ� ũ�⸦ 1�� ��ȯ�ϴ� �Ӽ�

        // �ʱ�ȭ
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;

        // ������Ʈ ���ư��� ���⿡ �°� ȸ�� ����
        // FromToRotation : ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        // �� ����
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
