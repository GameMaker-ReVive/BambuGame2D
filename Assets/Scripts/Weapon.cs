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
    [SerializeField] Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
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

        // player �� ������ �ִ� ��� Gear �� ���ؼ� ��� ApplyGear �Լ��� ����
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); // BroadcastMessage : Ư�� �Լ� ȣ���� ��� �ڽĿ��� ����ϴ� �Լ�
        // SendMessageOptions.DontRequireReceiver : ȣ���� �޴� ��ü�� �� �������� �ʾƵ� �ȴٶ�� �ɼ��� �߰�
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform; // ������ ������Ʈ�� �÷��̾� �ڽ����� ����
        transform.localPosition = Vector3.zero; // �÷��̾� �ȿ��� ��ġ�� (0, 0, 0) ���� ���߱⿡ ���� ��ġ�� LocalPosition �� �������� ����

        // Property Set
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for(int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            // ItemData ���� �����ص� �����հ� pool �� �ִ� �������� ���� ��� index ������ prefabId �� ����
            if(data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150; // Vector3.back �� ����ϱ⿡ ��������� �ð�������� ȸ��
                Batch();
                break;
            default:
                // speed ���� ����ӵ��� �ǹ� -> ���� ���� ���� �߻�
                speed = 0.3f;
                break;
        }

        // Hand Set
        Hand hand = player.hands[(int)data.itemType]; // enum �� �տ� (int) �� �ٿ��༭ ����ȯ 
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
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
